using Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Task.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>  options) : base (options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}