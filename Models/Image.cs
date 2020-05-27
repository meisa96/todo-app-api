using System;

namespace Task.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicID { get; set; }


        //start relations
        public virtual ToDo ToDo { get; set; }
        public int ToDoId { get; set; }
    }
}