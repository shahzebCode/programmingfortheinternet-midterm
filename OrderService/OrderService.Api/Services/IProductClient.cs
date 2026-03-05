namespace OrderService.Api.Services
{
    public interface IProductClient
    {
        Task<bool> ProductExistsAsync(int productId);
        Task<ProductDto?> GetProductAsync(int productId);
        
        Task<bool> UpdateProductAsync(ProductDto product);
    }
    public record ProductDto(int Id, string Name, decimal Price, int Stock);
}