using ChatGptMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatGptMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7243/api/Gym/chat"; // Cambia esto por la URL correcta de tu API

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequest chatRequest)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(chatRequest), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }

            return StatusCode((int)response.StatusCode, "Error al obtener respuesta del API.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
