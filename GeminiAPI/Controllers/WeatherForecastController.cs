using KOLAffiliate.API.Models.GeminiModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        private const string URL = "https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent";
        private string urlParameters = "?key=AIzaSyDB7OMHk6n3NbyeKlX_Hb1UtnB6reBjz9Q";

        private readonly JsonSerializerSettings _serializerSettings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        private async Task<string> CallGeminiAPI(string prompt)
        {
            GeminiRequest requestBody = new GeminiRequest();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                requestBody = GeminiRequestFactory.CreateRequest(prompt);
                var content = new StringContent(JsonConvert.SerializeObject(requestBody, Formatting.None, _serializerSettings), Encoding.UTF8, "application/json");
                var httpResonse = await httpClient.PostAsync(urlParameters, content);
                httpResonse.EnsureSuccessStatusCode();
                var responseBody = await httpResonse.Content.ReadAsStringAsync();
                var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseBody);
                return geminiResponse?.Candidates[0].Content.Parts[0].Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                var a = requestBody;
                return null;
            }

        }
    }
}
