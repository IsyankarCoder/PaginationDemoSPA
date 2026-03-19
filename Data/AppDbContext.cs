using Microsoft.EntityFrameworkCore;
using PaginationDemoSPA.Models;

namespace PaginationDemoSPA.Data
{
    public class AppDbContext
        :DbContext
    {

        public DbSet<Product> Products { get; set; }    
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }
    }
}
