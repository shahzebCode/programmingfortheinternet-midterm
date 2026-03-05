using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.Models;
using OrderService.Api.Services;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;
    private readonly ICustomerClient _customerClient;
    private readonly IProductClient _productClient;
    private readonly IPaymentClient _paymentClient;

    public record CreateOrderRequest(int CustomerId, int ProductId, int Quantity, bool IsPaid);

    public OrdersController(OrdersDbContext context, ICustomerClient customerClient, IProductClient productClient,
    IPaymentClient paymentClient)
    {
        _context = context;
        _customerClient = customerClient;
        _productClient = productClient;
        _paymentClient = paymentClient;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Orders.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest req)
    {
        var exists = await _customerClient.CustomerExistsAsync(req.CustomerId);
        if (!exists)
        {
            return BadRequest($"Customer with ID {req.CustomerId} does not exist.");
        }


        var productExists = await _productClient.ProductExistsAsync(req.ProductId);
        if (!productExists)
        {
            return BadRequest($"Product with ID {req.ProductId} does not exist.");
        }



        if (req.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than 0.");

        }
        var product = await _productClient.GetProductAsync(req.ProductId);
        if (product is null)
        {
            return BadRequest($"Product with ID {req.ProductId} does not exist.");
        }

        if (product.Stock < req.Quantity)
        {
            return BadRequest("Insufficient stock.");
        }

        // Calculating Total
        var total = product.Price * req.Quantity;


        var order = new Order
        {
            CustomerId = req.CustomerId,
            ProductId = req.ProductId,
            Quantity = req.Quantity,
            IsPaid = req.IsPaid,
            Total = total
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();



        // call PaymentService
        var payment = await _paymentClient.CreatePaymentAsync(order.Id, order.Total, order.IsPaid);




        // reduce product stock
        var updatedProduct = product with { Stock = product.Stock - req.Quantity };
        var stockUpdated = await _productClient.UpdateProductAsync(updatedProduct);

        var after = await _productClient.GetProductAsync(req.ProductId);
        Console.WriteLine($"[OrdersController] Stock after update = {after?.Stock}");

        if (!stockUpdated)
        {
            return StatusCode(500, "Failed to update product stock.");
        }

        // return both
        return Ok(new { order, payment });

        //return Ok(order);
    }
}
