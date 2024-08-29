using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OpenWeather_Imple.MyWeatherApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            string apiKey = "c50c31982275802438811b380f980525"; // Reemplaza con tu API Key de OpenWeather
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=es";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Ciudad no encontrada o error en la API");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var weatherData = JObject.Parse(jsonResponse);

            var result = new
            {
                City = weatherData["name"].ToString(),
                Temperature = weatherData["main"]["temp"].ToString() + " °C",
                Weather = weatherData["weather"][0]["description"].ToString(),
            };

            return Ok(result);
        }
    }
}

