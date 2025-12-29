namespace ShopifyHub.Application.DTOs
{
    public class SyncResultDto
    {
        public bool Success { get; set; }
        public int RecordsProcessed { get; set; }
        public int RecordsFailed { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
