using App2.Models;
using System.Text.Json;

namespace MVC.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:5001/api/orders");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Order>>(json, _options);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/orders/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Order>(json, _options);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/orders", order);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Order>(json, _options);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:5001/api/orders/{order.Id}", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
