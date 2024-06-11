using App2.Models;
using System.Text.Json;

namespace MVC.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<Category>> GetCategoryAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:5001/api/categories");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Category>>(json, _options);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/categories/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(json, _options);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/categories", category);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(json, _options);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:5001/api/categories/{category.Id}", category);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
