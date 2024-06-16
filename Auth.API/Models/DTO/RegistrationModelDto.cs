using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Auth.API.Models.DTO
{
    public class RegistrationModelDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(15, ErrorMessage = "Name length can't be more than 15.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(15, ErrorMessage = "User Name length can't be more than 15.")]
        public string? Username { get; set; }

        
        [Required(ErrorMessage = "The email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "Name length can't be more than 10.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?"":{}|<>])(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*(),.?"":{}|<>]{10,}$",
        ErrorMessage = "Password must contain at least 10 characters, including at least 1 uppercase letter, at least 1 lowercase letter, at least one number and a special character.")]
        [Required(ErrorMessage = "Password is required.")]
        
        public string? Password { get; set; }

        //public string[] Roles { get; set; }
    }
}
