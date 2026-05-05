using System.ComponentModel.DataAnnotations;

namespace AIApi.Models
{
    public class AIFramework
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // one-to-many relationship with AIModel
        public ICollection<AIModel> Models { get; set; } = new List<AIModel>();
    }
}