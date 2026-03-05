using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Api.Data;
using PaymentService.Api.Models;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsDbContext _db;

        public PaymentsController(PaymentsDbContext db) => _db = db;

        public record CreatePaymentRequest(int OrderId, decimal Amount, bool IsPaid);

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentRequest req)
        {
            // mock payment decision (simple + demo-friendly)
            var status = req.Amount <= 1000 ? "Paid" : "Failed";

            var isPaid = req.IsPaid;

            var payment = new Payment
            {
                OrderId = req.OrderId,
                Amount = req.Amount,
                Status = status,
                IsPaid = isPaid
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return Ok(payment);
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var p = await _db.Payments.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            return p is null ? NotFound() : Ok(p);
        }
    }
}