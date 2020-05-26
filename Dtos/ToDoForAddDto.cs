using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Task.Dtos
{
    public class ToDoForAddDto
    {
        public int UserId { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]

        public DateTime DateTime { get; set; }

    }
}