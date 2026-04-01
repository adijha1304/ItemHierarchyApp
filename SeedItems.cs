using ItemHierarchyApp.Models;
using ItemHierarchyApp.Data;  // ✅ Add this line
using System.Threading.Tasks;
using System.Linq;

public static class SeedItems
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Items.Any()) return; // Skip if database already has data

        // Create parent item
        var parent = new Item { Name = "Parent Item", Weight = 10 };

        // Create child items
        var child1 = new Item { Name = "Child Item 1", Weight = 5, Parent = parent };
        var child2 = new Item { Name = "Child Item 2", Weight = 3, Parent = parent };

        // Add all items
        db.Items.AddRange(parent, child1, child2);

        await db.SaveChangesAsync();
    }
}