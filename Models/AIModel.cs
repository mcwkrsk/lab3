// Models/AIModel.cs
using System.ComponentModel.DataAnnotations;

namespace AIApi.Models
{
    public class AIModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Version { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // Outer key for AIFramework foreign key relationship (one-to-many)
        public int FrameworkId { get; set; }
        public AIFramework Framework { get; set; } = null!;

        // Many-to-many relationship with Dataset through ModelDataset join table
        public ICollection<ModelDataset> ModelDatasets { get; set; } = new List<ModelDataset>();
    }
}