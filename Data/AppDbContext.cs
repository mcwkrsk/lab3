using Microsoft.EntityFrameworkCore;
using AIApi.Models;

namespace AIApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AIFramework> Frameworks { get; set; }
        public DbSet<AIModel> Models { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<ModelDataset> ModelDatasets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Составной первичный ключ для промежуточной таблицы
            modelBuilder.Entity<ModelDataset>()
                .HasKey(md => new { md.ModelId, md.DatasetId });

            // Связь "один ко многим": Framework -> Model
            modelBuilder.Entity<AIModel>()
                .HasOne(m => m.Framework)
                .WithMany(f => f.Models)
                .HasForeignKey(m => m.FrameworkId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка many-to-many через ModelDataset
            modelBuilder.Entity<ModelDataset>()
                .HasOne(md => md.Model)
                .WithMany(m => m.ModelDatasets)
                .HasForeignKey(md => md.ModelId);

            modelBuilder.Entity<ModelDataset>()
                .HasOne(md => md.Dataset)
                .WithMany(d => d.ModelDatasets)
                .HasForeignKey(md => md.DatasetId);
        }
    }
}