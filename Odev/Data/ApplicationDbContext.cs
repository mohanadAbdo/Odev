using Microsoft.EntityFrameworkCore;
using Odev.Models;

namespace Odev.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
    }
}
