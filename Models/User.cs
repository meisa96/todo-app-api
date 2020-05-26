using System;
using System.Collections;
using System.Collections.Generic;

namespace Task.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }

    }
}