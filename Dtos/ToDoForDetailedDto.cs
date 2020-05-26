using System;
using System.Collections.Generic;
using Task.Models;

namespace Task.Dtos
{
    public class ToDoForDetailedDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDone { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<ImagesForDetailedDto> Images { get; set; }
    }
}