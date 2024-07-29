using Microsoft.AspNetCore.Mvc;
using APIChatGpt.Models;
using APIChatGpt.Services;
using APIChatGpt.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APIChatGpt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymController : ControllerBase
    {
        private readonly GymContext _context;
        private readonly OpenAIService _openAIService;

        public GymController(GymContext context, OpenAIService openAIService)
        {
            _context = context;
            _openAIService = openAIService;
        }

        /// <summary>
        /// Obtiene una respuesta a una pregunta relacionada con el gimnasio.
        /// </summary>
        /// <param name="userInputModel">El modelo que contiene la pregunta del usuario.</param>
        /// <returns>La respuesta correspondiente.</returns>
        [HttpPost("chat")]
        public async Task<IActionResult> GetChatResponse([FromBody] UserInputModel userInputModel)
        {
            var userInput = userInputModel.UserInput;
            var response = "Lo siento, solo puedo ayudarte con temas relacionados con el gimnasio.";

            if (IsGymRelated(userInput))
            {
                var gymDataResponse = await GetGymDataResponse(userInput);
                if (!string.IsNullOrEmpty(gymDataResponse))
                {
                    response = gymDataResponse;
                }
                else
                {
                    response = await _openAIService.GetResponseAsync(userInput);
                }
            }

            return Ok(response);
        }

        private bool IsGymRelated(string input)
        {
            var gymKeywords = new[] { "ejercicio", "máquina", "entrenador", "gimnasio", "servicio" };
            return gymKeywords.Any(keyword => input.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> GetGymDataResponse(string input)
        {
            if (input.Contains("ejercicio", StringComparison.OrdinalIgnoreCase))
            {
                var exercises = await _context.Exercises.ToListAsync();
                return $"Ejercicios disponibles: {string.Join(", ", exercises.Select(e => e.Name))}";
            }

            if (input.Contains("máquina", StringComparison.OrdinalIgnoreCase))
            {
                var machines = await _context.Machines.ToListAsync();
                return $"Máquinas disponibles: {string.Join(", ", machines.Select(m => m.Name))}";
            }

            if (input.Contains("entrenador", StringComparison.OrdinalIgnoreCase))
            {
                var trainers = await _context.Trainers.ToListAsync();
                return $"Entrenadores disponibles: {string.Join(", ", trainers.Select(t => t.Name))}";
            }

            if (input.Contains("servicio", StringComparison.OrdinalIgnoreCase))
            {
                var services = await _context.Services.ToListAsync();
                return $"Servicios disponibles: {string.Join(", ", services.Select(s => s.Name))}";
            }

            return null;
        }
    }
}
