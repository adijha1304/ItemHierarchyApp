using Microsoft.EntityFrameworkCore;
using ItemHierarchyApp.Models; // if you have models here

namespace ItemHierarchyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Example DbSet
        public DbSet<Item> Items { get; set; }
    }
}