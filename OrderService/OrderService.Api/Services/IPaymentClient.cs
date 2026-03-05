namespace OrderService.Api.Services
{
    public interface IPaymentClient
    {
        Task<PaymentResult> CreatePaymentAsync(int orderId, decimal amount, bool isPaid);
    }

    public record PaymentResult(int Id, int OrderId, decimal Amount, string Status, DateTime CreatedAt);
}