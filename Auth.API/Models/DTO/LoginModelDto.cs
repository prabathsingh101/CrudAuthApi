using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.DTO
{
    public class LoginModelDto
    {
        [Required]
        public string? Username { get; set; }
       
        [Required]
        public string? Password { get; set; }
    }
}
