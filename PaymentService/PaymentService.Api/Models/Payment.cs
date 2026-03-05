namespace PaymentService.Api.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Paid"; // Paid / Failed

        public bool IsPaid {get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}