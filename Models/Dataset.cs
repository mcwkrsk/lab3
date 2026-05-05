using System.ComponentModel.DataAnnotations;

namespace AIApi.Models
{
    public class Dataset
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Source { get; set; }

        public long? SizeInMb { get; set; }

        // Many-to-many relationship with AIModel through ModelDataset join table
        public ICollection<ModelDataset> ModelDatasets { get; set; } = new List<ModelDataset>();
    }
}