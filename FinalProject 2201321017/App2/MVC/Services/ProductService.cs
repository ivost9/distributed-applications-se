using App2.Models;
using System.Net.Http;
using System.Text.Json;

namespace MVC.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:5001/api/products");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(json, _options);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/products/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(json, _options);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/products", product);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(json, _options);
        }

        public async Task UpdateProductAsync(Product book)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:5001/api/products/{book.Id}", book);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
