using genealogy_ssr.Services;
using Microsoft.AspNetCore.Mvc;

namespace genealogy_ssr.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		#region Constructor
		private readonly IWeatherForecastService weatherForecastService;
		private readonly ILogger<WeatherForecastController> logger;
		public WeatherForecastController(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastController> logger)
		{
			this.weatherForecastService = weatherForecastService;
			this.logger = logger;
		}
		#endregion

		[HttpGet]
		public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
		{
			var forecasts = await weatherForecastService.GetWeatherForecasts();
			return Ok(forecasts);
		}
	}
}