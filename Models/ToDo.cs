using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        //Relations//
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}