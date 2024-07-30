using Microsoft.EntityFrameworkCore;
using APIChatGpt.Models;

namespace APIChatGpt.Data
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options) : base(options) { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<FitnessProgram> FitnessPrograms { get; set; }
        public DbSet<HealthService> HealthServices { get; set; }
    }
}
