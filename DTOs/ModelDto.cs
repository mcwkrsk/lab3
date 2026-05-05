namespace AIApi.DTOs
{
    public class ModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Version { get; set; }
        public string? Description { get; set; }
        public int FrameworkId { get; set; }
        public string FrameworkName { get; set; } = string.Empty;
        public List<string> DatasetNames { get; set; } = new List<string>();
    }

    public class ModelCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Version { get; set; }
        public string? Description { get; set; }
        public int FrameworkId { get; set; }
        public List<int> DatasetIds { get; set; } = new List<int>(); // ID many-to-many
    }

    public class ModelUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Version { get; set; }
        public string? Description { get; set; }
        public int FrameworkId { get; set; }
        public List<int> DatasetIds { get; set; } = new List<int>();
    }
}