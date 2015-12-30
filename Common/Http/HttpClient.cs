using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Common.Json;

namespace Common.Http
{
    public class HttpClient : HttpClientBase, IHttpClient
    {
        private string _token;

        public HttpClient()
            : base(new JsonSerializer(), new JsonSerializer())
        {
        }

        public HttpClient(ISerializer serializer = null)
            : base(serializer ?? new JsonSerializer(), serializer ?? new JsonSerializer())
        {
        }

        public HttpClient(ISerializer requestSerializer = null, ISerializer responseSerializer = null)
            : base(requestSerializer ?? new JsonSerializer(), responseSerializer ?? new JsonSerializer())
        {
        }

        public IHttpClient SetToken(string token)
        {
            _token = token;
            return this;
        }

        public HttpClient SetSerializers(ISerializer requestSerializer = null, ISerializer responseSerializer = null)
        {
            _requestSerializer = requestSerializer;
            _responseSerializer = responseSerializer;
            return this;
        }

        public async Task<HttpResponse<T>> GetAsync<T>(Uri url)
        {
            var request = await InitRequestAsync(url, "GET");
            return await GetResponseAsync<T>(request);
        }

        public Task<HttpResponse<T>> PostAsync<T>(Uri url, T data)
        {
            return PostAsync<T, T>(url, data);
        }

        public async Task<HttpResponse<TOut>> PostAsync<TIn, TOut>(Uri url, TIn data)
        {
            var request = await InitRequestAsync(url, "POST", data);
            return await GetResponseAsync<TOut>(request);
        }

        public Task<HttpResponse<T>> PutAsync<T>(Uri url, T data)
        {
            return PutAsync<T, T>(url, data);
        }

        public async Task<HttpResponse<TOut>> PutAsync<TIn, TOut>(Uri url, TIn data)
        {
            var request = await InitRequestAsync(url, "PUT", data);
            return await GetResponseAsync<TOut>(request);
        }

        public async Task<HttpResponse<T>> DeleteAsync<T>(Uri url)
        {
            var request = await InitRequestAsync(url, "DELETE");
            return await GetResponseAsync<T>(request);
        }

        public async Task<HttpStatusCode> DeleteAsync(Uri url)
        {
            var request = await InitRequestAsync(url, "DELETE");
            return (await GetResponseAsync<object>(request)).StatusCode;
        }

        private async Task<HttpResponse<T>> GetResponseAsync<T>(WebRequest request)
        {
            HttpWebResponse response = null;
            WebException webException = null;
            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException exception)
            {
                webException = exception;
            }

            if (webException != null)
            {
                response = webException.Response as HttpWebResponse;
                if (response != null)
                {
                    string rawBody;
                    try
                    {
                        var responseStream = response.GetResponseStream();
                        rawBody = await DeserializeDataAsync<string>(responseStream);
                    }
                    catch (Exception)
                    {
                        rawBody = null;
                    }

                    throw new HttpException(response.StatusCode, rawBody, request.RequestUri, webException);
                }

                throw new HttpException(webException);
            }

            var reponseStream = response.GetResponseStream();
            if (reponseStream == null)
            {
                return new HttpResponse<T>(response.StatusCode, response.Headers, default(T), request.RequestUri);
            }

            return new HttpResponse<T>(response.StatusCode, response.Headers, await DeserializeDataAsync<T>(reponseStream), request.RequestUri);
        }

        private async Task<HttpWebRequest> InitRequestAsync<T>(Uri uri, string method, T data)
        {
            var ret = WebRequest.CreateHttp(uri);
            ret.ContentType = GetContentType();
            ret.Accept = GetAccept();

            if (_token != null)
            {
                ret.Headers.Add("Authorization", "Bearer " + _token);
            }

            ret.Method = method;

            if (data != null)
            {
                await InitRequestBodyAsync(ret, data);
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ret.Proxy = null;   // Perf gain by forcing to NULL as per http://en.code-bude.net/2013/01/21/3-things-you-should-know-to-speed-up-httpwebrequest/

            return ret;
        }

        private async Task<HttpWebRequest> InitRequestAsync(Uri uri, string method)
        {
            return await InitRequestAsync(uri, method, (object)null);
        }

        private async Task InitRequestBodyAsync<T>(WebRequest request, T data)
        {
            var raw = await SerializeDataAsync(data);
            using (var writer = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                await writer.WriteAsync(raw);
            }
        }

        public HttpResponse<T> Get<T>(Uri url)
        {
            var request = InitRequest(url, "GET");
            return GetResponse<T>(request);
        }

        public HttpResponse<T> Post<T>(Uri url, T data)
        {
            return Post<T, T>(url, data);
        }

        public HttpResponse<TOut> Post<TIn, TOut>(Uri url, TIn data)
        {
            var request = InitRequest(url, "POST", data);
            return GetResponse<TOut>(request);
        }

        public HttpResponse<T> Put<T>(Uri url, T data)
        {
            return Put<T, T>(url, data);
        }

        public HttpResponse<TOut> Put<TIn, TOut>(Uri url, TIn data)
        {
            var request = InitRequest(url, "PUT", data);
            return GetResponse<TOut>(request);
        }

        public HttpResponse<T> Delete<T>(Uri url)
        {
            var request = InitRequest(url, "DELETE");
            return GetResponse<T>(request);
        }

        public HttpStatusCode Delete(Uri url)
        {
            var request = InitRequest(url, "DELETE");
            return (GetResponse<object>(request)).StatusCode;
        }

        private HttpResponse<T> GetResponse<T>(WebRequest request)
        {
            HttpWebResponse response = null;
            WebException webException = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                webException = exception;
            }

            if (webException != null)
            {
                response = webException.Response as HttpWebResponse;
                if (response != null)
                {
                    string rawBody;
                    try
                    {
                        var responseStream = response.GetResponseStream();
                        rawBody = DeserializeData<string>(responseStream);
                    }
                    catch (Exception)
                    {
                        rawBody = null;
                    }

                    throw new HttpException(response.StatusCode, rawBody, request.RequestUri, webException);
                }

                throw new HttpException(webException);
            }

            var reponseStream = response.GetResponseStream();
            if (reponseStream == null)
            {
                return new HttpResponse<T>(response.StatusCode, response.Headers, default(T), request.RequestUri);
            }

            return new HttpResponse<T>(response.StatusCode, response.Headers, DeserializeData<T>(reponseStream), request.RequestUri);
        }

        private HttpWebRequest InitRequest<T>(Uri uri, string method, T data)
        {
            var ret = WebRequest.CreateHttp(uri);
            ret.ContentType = GetContentType();
            ret.Accept = GetAccept();

            if (_token != null)
            {
                ret.Headers.Add("Authorization", "Bearer " + _token);
            }

            ret.Method = method;

            if (data != null)
            {
                InitRequestBody(ret, data);
            }

            return ret;
        }

        private HttpWebRequest InitRequest(Uri uri, string method)
        {
            return InitRequest(uri, method, (object)null);
        }

        private void InitRequestBody<T>(WebRequest request, T data)
        {
            var raw = SerializeData(data);
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteAsync(raw);
            }
        }
    }
}
