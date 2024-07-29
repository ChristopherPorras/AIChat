using Microsoft.EntityFrameworkCore;
using APIChatGpt.Models;

namespace APIChatGpt.Data
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options) : base(options) { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<APIChatGpt.Models.Service> Services { get; set; } // Usa el namespace completo para evitar conflicto
    }
}
