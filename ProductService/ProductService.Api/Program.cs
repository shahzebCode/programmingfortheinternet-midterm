using Microsoft.EntityFrameworkCore;
using ProductService.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite DB-per-service
builder.Services.AddDbContext<ProductDbContext>(options =>
    //options.UseSqlite("Data Source=products.db"));
    options.UseSqlite("Data Source=Data/products.db"));

var app = builder.Build();

// apply migrations (better than EnsureCreated for grading)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();