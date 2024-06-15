using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.DTO
{
    public class ImageUploadRequestDto
    {
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
