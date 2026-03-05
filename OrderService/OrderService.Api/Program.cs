using OrderService.Api.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrdersDbContext>(options =>
    //options.UseSqlite("Data Source=orders.db"));
    options.UseSqlite("Data Source=Data/orders.db"));

var customerServiceUrl = builder.Configuration["CustomerServiceUrl"];

builder.Services.AddHttpClient<ICustomerClient, CustomerClient>(client =>
{
    client.BaseAddress = new Uri(customerServiceUrl!);
});

var productServiceUrl = builder.Configuration["ProductServiceUrl"];

builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
{
    client.BaseAddress = new Uri(productServiceUrl!);
});

var paymentServiceUrl = builder.Configuration["PaymentServiceUrl"];

builder.Services.AddHttpClient<IPaymentClient, PaymentClient>(client =>
{
    client.BaseAddress = new Uri(paymentServiceUrl!);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();