using System;
using Microsoft.AspNetCore.Http;

namespace Task.Dtos
{
    public class ImageForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
    }
}