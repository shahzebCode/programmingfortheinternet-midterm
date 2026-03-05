namespace OrderService.Api.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;

        public ProductClient(HttpClient http) => _http = http;

        public async Task<bool> ProductExistsAsync(int productId)
        {
            var res = await _http.GetAsync($"/api/products/{productId}/exists");
            return res.IsSuccessStatusCode;
        }
        public async Task<ProductDto?> GetProductAsync(int productId)
        {
            // This calls ProductService's GET /api/products/{id}
            return await _http.GetFromJsonAsync<ProductDto>($"/api/products/{productId}");
        }

        /*public async Task<bool> UpdateProductAsync(ProductDto product)
        {
            var response = await _http.PutAsJsonAsync($"/api/products/{product.Id}", product);
            return response.IsSuccessStatusCode;
        }*/

        public async Task<bool> UpdateProductAsync(ProductDto product)
        {
            Console.WriteLine($"[ProductClient] PUT /api/products/{product.Id} stock={product.Stock}");

            var response = await _http.PutAsJsonAsync($"/api/products/{product.Id}", product);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ProductClient] FAILED {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
            }
            else
            {
                Console.WriteLine($"[ProductClient] OK {(int)response.StatusCode} {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}