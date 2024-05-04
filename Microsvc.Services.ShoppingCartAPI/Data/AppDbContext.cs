using Microsoft.EntityFrameworkCore;
using Microsvc.Services.ShoppingCartAPI.Models;

namespace Micorsvc.Services.ShoppingCartAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }


    }
}
