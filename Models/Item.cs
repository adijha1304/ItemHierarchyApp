using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemHierarchyApp.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Weight is required")]
        [Range(0, 1000, ErrorMessage = "Weight must be between 0 and 1000")]
        [Column(TypeName = "REAL")]
        public decimal Weight { get; set; }

        public int? ParentId { get; set; }

        public Item? Parent { get; set; }

        public List<Item> Children { get; set; } = new List<Item>();
    }
}