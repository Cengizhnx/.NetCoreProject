using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }
        public int Total { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    }
}
