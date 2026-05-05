namespace AIApi.DTOs
{
    public class DatasetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Source { get; set; }
        public long? SizeInMb { get; set; }
    }

    public class DatasetCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Source { get; set; }
        public long? SizeInMb { get; set; }
    }

    public class DatasetUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Source { get; set; }
        public long? SizeInMb { get; set; }
    }
}