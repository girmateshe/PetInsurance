using System;
using System.Net;

namespace Common.Http
{
    public class HttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; private set; }

        public WebHeaderCollection Headers { get; private set; }

        public T Content { get; private set; }

        public Uri RequestUri { get; private set; }

        public HttpResponse(HttpStatusCode statusCode, WebHeaderCollection headers, T content = default(T), Uri requestUri = null)
        {
            StatusCode = statusCode;
            Headers = headers;
            Content = content;
            RequestUri = requestUri;
        }
    }
}
