using System.Net;

namespace AppX.Client.Domain.Entities.Common
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
        public MetaData? MetaData { get; set; }
        public string? ResponseMessage { get; set; }
        public bool Success { get; set; } = false;
    }

    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public MetaData? MetaData { get; set; }
        public string? ResponseMessage { get; set; }

        public bool Success { get; set; }
    }
}
