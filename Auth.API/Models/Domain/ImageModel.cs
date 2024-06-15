﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.API.Models.Domain
{
    public class ImageModel
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string FileName { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeBytes { get; set; }

        public string FilePath { get; set; }
    }
}
