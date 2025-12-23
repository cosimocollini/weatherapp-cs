namespace BlazorWeatherApp.Services;

using Microsoft.EntityFrameworkCore;
using BlazorWeatherApp.Data;
using Microsoft.Extensions.Options;
using BlazorWeatherApp.Models;
public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;
    private readonly AppDbContext _context;

    public WeatherService(HttpClient httpClient, IOptions<WeatherApiOptions> options, AppDbContext context)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _context = context;

    }

    public async Task<WeatherResponse> GetWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new InvalidOperationException("Weather API key not configured. Imposta WeatherApi:ApiKey in user-secrets o in appsettings.");

        var url = $"?key={Uri.EscapeDataString(_options.ApiKey)}&q={Uri.EscapeDataString(city)}";
        var weather = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
        return weather;
    }
    // public async Task<WeatherResponse> GetWeatherAsync(string city)
    // {

    //     var weather = await _httpClient.GetFromJsonAsync<WeatherResponse>($"?key={_options.ApiKey}&q={city}");
    //     return weather;
    // }

    public async Task<List<Favourites>> GetFavList()
    {
        return await _context.Favourites
            .Where(u => u.Name != null)
            .OrderBy(u => u.Name)
            .ToListAsync();

    }

    public async Task AddToFavourites(string cityName)
    {
        var favourite = new Favourites
        {
            Id = new Random().Next(1000, 9999),
            Name = cityName
        };

        _context.Favourites.Add(favourite);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromFavourites(string cityName)
    {
        var favourite = await _context.Favourites
            .FirstOrDefaultAsync(f => f.Name == cityName);

        if (favourite != null)
        {
            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();
        }
    }
}

// public class Favourite
// {
//     public string Name { get; set; } = string.Empty;
//     public int Id { get; set; }
// }