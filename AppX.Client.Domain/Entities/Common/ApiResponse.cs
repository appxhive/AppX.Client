using System.Net;

namespace AppX.Client.Domain.Entities.Common
{
    public class ApiResponse
    {
        private HttpStatusCode _statusCode;
        private bool _success;
        public HttpStatusCode StatusCode 
        { 
            get{  return _statusCode; }
            set { _statusCode = value; }
        }
        public object? Data { get; set; }
        public MetaData? MetaData { get; set; }
        public string? ResponseMessage { get; set; }
        public bool Success 
        {
            get { return _success; } 
            set
            {
                _success = (_statusCode == HttpStatusCode.OK) ? true : false;
            }
        }
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
