namespace EnsekTask.Models.Responses
{
    public class MeterReadingUploadResponse
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public string Error { get; set; } = null!;
    }
}
