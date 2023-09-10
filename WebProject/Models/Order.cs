namespace WebProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Product { get; set; }
        public int Total { get; set; }
        public string Status { get; set; } = "Waiting";
        public DateTime OrderCreatedDateTime { get; set; } = DateTime.Now;
    }
}
