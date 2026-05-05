namespace AIApi.Models
{
    public class ModelDataset
    {
        public int ModelId { get; set; }
        public AIModel Model { get; set; } = null!;

        public int DatasetId { get; set; }
        public Dataset Dataset { get; set; } = null!;
    }
}