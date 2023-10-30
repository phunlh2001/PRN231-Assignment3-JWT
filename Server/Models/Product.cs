using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Server.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Name must be not empty.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category must be not empty.")]
        public string Category { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "Price must be greater than 0.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal UnitPrice { get; set; }
        [Required(ErrorMessage = "Quantity must be greater than 0.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int AvailableQuantity { get; set; }
    }
}
