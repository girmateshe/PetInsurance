using System;
using System.Net;

namespace Common.Http
{
    public class HttpException : Exception
    {
        public HttpStatusCode? StatusCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public string Url { get; set; }

        public HttpException(HttpStatusCode statusCode, string errorMessage, Uri url, Exception innerException)
            : base(String.Format("Request failed with status code:{0} ({1}). Url: {2}", (int)statusCode, statusCode, url), innerException)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public HttpException(Exception innerException)
            : base("Request failed", innerException)
        {
        }
    }
}
