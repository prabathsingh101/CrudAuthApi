using Auth.API.Data;
using Auth.API.Models.Domain;
using Auth.API.Repositories.Abstract;

namespace Auth.API.Repositories.Domain
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DatabaseContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            DatabaseContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<ImageModel> Upload(ImageModel image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
               $"{image.FileName}{image.FileExtension}");

            //upload image to local path

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/Images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //Add the image into image table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
