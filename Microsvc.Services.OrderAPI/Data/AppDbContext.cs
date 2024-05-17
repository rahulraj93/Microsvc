using Microsoft.EntityFrameworkCore;
using Microsvc.Services.OrderAPI.Models;

namespace Micorsvc.Services.OrderAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }


    }
}
