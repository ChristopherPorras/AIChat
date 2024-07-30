using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIChatGpt.Data;
using APIChatGpt.Models;
using APIChatGpt.Services;

namespace APIChatGpt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymController : ControllerBase
    {
        private readonly GymContext _context;
        private readonly OpenAIService _openAIService;
        private static List<string> _previousQuestions = new List<string>();

        public GymController(GymContext context, OpenAIService openAIService)
        {
            _context = context;
            _openAIService = openAIService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> GetChatResponse([FromBody] UserInputModel userInputModel)
        {
            if (userInputModel == null || string.IsNullOrWhiteSpace(userInputModel.UserInput))
            {
                return BadRequest("Input cannot be empty.");
            }

            var userInput = userInputModel.UserInput;
            _previousQuestions.Add(userInput);
            var contextPrompt = string.Join("\n", _previousQuestions);
            var gymDataResponse = await GetGymDataResponse(userInput);

            string response;

            if (IsFitnessRelated(userInput))
            {
                string combinedPrompt = string.IsNullOrEmpty(gymDataResponse)
                    ? contextPrompt
                    : $"{contextPrompt}\n{gymDataResponse}";

                response = await _openAIService.GetResponseAsync(combinedPrompt);
            }
            else
            {
                response = "Lo siento, solo puedo ayudarte con temas relacionados con el gimnasio y el fitness.";
            }

            return Ok(new { response });
        }

        private bool IsFitnessRelated(string input)
        {
            var fitnessKeywords = new[] { "ejercicio", "máquina", "entrenador", "gimnasio", "servicio", "músculo", "salud", "rutina", "entrenamiento", "dieta", "fitness" };
            return fitnessKeywords.Any(keyword => input.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> GetGymDataResponse(string input)
        {
            if (input.Contains("ejercicio", StringComparison.OrdinalIgnoreCase))
            {
                var exercises = await _context.Exercises.ToListAsync();
                return exercises.Any() ? $"Ejercicios disponibles: {string.Join(", ", exercises.Select(e => e.Name))}" : "No hay ejercicios disponibles en la base de datos.";
            }

            if (input.Contains("máquina", StringComparison.OrdinalIgnoreCase))
            {
                var machines = await _context.Machines.ToListAsync();
                return machines.Any() ? $"Máquinas disponibles: {string.Join(", ", machines.Select(m => m.Name))}" : "No hay máquinas disponibles en la base de datos.";
            }

            if (input.Contains("entrenador", StringComparison.OrdinalIgnoreCase))
            {
                var trainers = await _context.Trainers.ToListAsync();
                return trainers.Any() ? $"Entrenadores disponibles: {string.Join(", ", trainers.Select(t => t.Name))}" : "No hay entrenadores disponibles en la base de datos.";
            }

            if (input.Contains("servicio", StringComparison.OrdinalIgnoreCase))
            {
                var services = await _context.Services.ToListAsync();
                return services.Any() ? $"Servicios disponibles: {string.Join(", ", services.Select(s => s.Name))}" : "No hay servicios disponibles en la base de datos.";
            }

            if (input.Contains("músculo", StringComparison.OrdinalIgnoreCase))
            {
                var muscles = await _context.Muscles.ToListAsync();
                return muscles.Any() ? $"Músculos principales: {string.Join(", ", muscles.Select(m => m.Name))}" : "No hay músculos disponibles en la base de datos.";
            }

            if (input.Contains("rutina", StringComparison.OrdinalIgnoreCase))
            {
                var routines = await _context.Routines.ToListAsync();
                return routines.Any() ? $"Rutinas disponibles: {string.Join(", ", routines.Select(r => r.Name))}" : "No hay rutinas disponibles en la base de datos.";
            }

            if (input.Contains("entrenamiento", StringComparison.OrdinalIgnoreCase))
            {
                var trainings = await _context.Trainings.ToListAsync();
                return trainings.Any() ? $"Planes de entrenamiento disponibles: {string.Join(", ", trainings.Select(t => t.Name))}" : "No hay planes de entrenamiento disponibles en la base de datos.";
            }

            if (input.Contains("dieta", StringComparison.OrdinalIgnoreCase))
            {
                var diets = await _context.Diets.ToListAsync();
                return diets.Any() ? $"Planes de dieta disponibles: {string.Join(", ", diets.Select(d => d.Name))}" : "No hay planes de dieta disponibles en la base de datos.";
            }

            if (input.Contains("fitness", StringComparison.OrdinalIgnoreCase))
            {
                var fitnessPrograms = await _context.FitnessPrograms.ToListAsync();
                return fitnessPrograms.Any() ? $"Programas de fitness disponibles: {string.Join(", ", fitnessPrograms.Select(fp => fp.Name))}" : "No hay programas de fitness disponibles en la base de datos.";
            }

            if (input.Contains("salud", StringComparison.OrdinalIgnoreCase))
            {
                var healthServices = await _context.HealthServices.ToListAsync();
                return healthServices.Any() ? $"Servicios de salud disponibles: {string.Join(", ", healthServices.Select(hs => hs.Name))}" : "No hay servicios de salud disponibles en la base de datos.";
            }

            return null;
        }

        // Métodos GET para verificar datos en la base de datos
        [HttpGet("exercises")]
        public async Task<IActionResult> GetExercises()
        {
            var exercises = await _context.Exercises.ToListAsync();
            return Ok(exercises);
        }

        [HttpGet("diets")]
        public async Task<IActionResult> GetDiets()
        {
            var diets = await _context.Diets.ToListAsync();
            return Ok(diets);
        }
    }
}
