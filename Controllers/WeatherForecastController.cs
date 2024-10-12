using Microsoft.AspNetCore.Mvc;

namespace DelusiveVortexAspApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return new List<WeatherForecast>
            {
                new WeatherForecast { Date = "2024-05-17", TemperatureC = 25 },
                new WeatherForecast { Date = "2024-05-18", TemperatureC = 22 }
            };
        }
    }

    public class WeatherForecast
    {
        public string Date { get; set; }
        public int TemperatureC { get; set; }
    }
}
