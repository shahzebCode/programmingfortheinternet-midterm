using Microsoft.EntityFrameworkCore;
using PaymentService.Api.Models;

namespace PaymentService.Api.Data
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options) { }
        public DbSet<Payment> Payments => Set<Payment>();
    }
}