using System.Net.Http.Json;

namespace OrderService.Api.Services
{
    public class PaymentClient : IPaymentClient
    {
        private readonly HttpClient _http;

        public PaymentClient(HttpClient http) => _http = http;

        public async Task<PaymentResult> CreatePaymentAsync(int orderId, decimal amount, bool isPaid)
        {
            var body = new { OrderId = orderId, Amount = amount, IsPaid = isPaid };

            var res = await _http.PostAsJsonAsync("/api/payments", body);
            res.EnsureSuccessStatusCode();

            var payment = await res.Content.ReadFromJsonAsync<PaymentResult>();
            if (payment is null) throw new Exception("PaymentService returned empty response.");

            return payment;
        }
    }
}