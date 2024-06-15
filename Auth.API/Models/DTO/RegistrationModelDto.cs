﻿using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models.DTO
{
    public class RegistrationModelDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        public string[] Roles { get; set; }
    }
}
