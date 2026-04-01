using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ItemHierarchyApp.Data;
using ItemHierarchyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemHierarchyApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        // List all items
       
public async Task<IActionResult> Index(string searchString)
{
    IQueryable<Item> items = _context.Items;

    if (!string.IsNullOrWhiteSpace(searchString))
    {
        items = items.Where(i => i.Name.Contains(searchString));
    }

    return View(await items.ToListAsync());
}
        // Create Item GET
        public IActionResult Create()
        {
            return View();
        }

        // Create Item POST
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // Edit GET
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // Delete GET
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var item = await _context.Items
        .Include(i => i.Children)
        .FirstOrDefaultAsync(i => i.Id == id);

    if (item != null)
    {
        // Delete children first
        if (item.Children != null && item.Children.Any())
        {
            _context.Items.RemoveRange(item.Children);
        }

        // Then delete parent
        _context.Items.Remove(item);

        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

        // ✅ Process Item GET
        public async Task<IActionResult> Process(int id)
        {
            var parentItem = await _context.Items
                .Include(i => i.Children)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (parentItem == null) return NotFound();

            return View(parentItem);
        }

        // ✅ Process Item POST (add children)
        [HttpPost]
        public async Task<IActionResult> Process(int parentId, List<Item> children)
        {
            var parentItem = await _context.Items.FindAsync(parentId);
            if (parentItem == null) return NotFound();

            foreach (var child in children)
            {
                child.ParentId = parentId;
                _context.Items.Add(child);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Tree View GET
        public async Task<IActionResult> Tree()
        {
            var items = await _context.Items
                .Include(i => i.Children)
                .Where(i => i.ParentId == null)
                .ToListAsync();

            return View(items);
        }

        // ✅ Processed Items (NEW)
public async Task<IActionResult> Processed()
{
    var items = await _context.Items
        .Where(i => i.ParentId != null) // only children items
        .ToListAsync();

    return View(items);
}
    }
}