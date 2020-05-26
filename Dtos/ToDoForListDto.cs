using System;

namespace Task.Dtos
{
    public class ToDoForListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDone { get; set; }
        public string PhotoUrl { get; set; }
    }
}