using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public List<String>? Images { get; set; }

    }
}
