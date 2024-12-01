using System.Diagnostics.CodeAnalysis;

namespace EnsekTask.Models.ViewModels
{
    //  nothing that can be tested here really
    [ExcludeFromCodeCoverage]
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
