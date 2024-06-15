using Auth.API.Models.Domain;

namespace Auth.API.Repositories.Abstract
{
    public interface IImageRepository
    {
        Task<ImageModel> Upload(ImageModel image);
    }
}
