using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
    public abstract class HttpClientBase
    {
        protected ISerializer _requestSerializer;
        protected ISerializer _responseSerializer;

        protected HttpClientBase(ISerializer requestSerializer, ISerializer responseSerializer)
        {
            _requestSerializer = requestSerializer;
            _responseSerializer = responseSerializer;

            ServicePointManager.DefaultConnectionLimit = 50;
            ServicePointManager.Expect100Continue = false;
        }

        protected async Task<string> SerializeDataAsync<T>(T data)
        {
            if (data is string)
            {
                return data as string;
            }

            if (data == null)
            {
                return String.Empty;
            }

            return await _requestSerializer.SerializeAsync(data);
        }

        protected async Task<T> DeserializeDataAsync<T>(Stream stream)
        {
            if (typeof(T) == typeof(Stream))
            {
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)(object)ms;
            }

            string raw;
            using (var reader = new StreamReader(stream))
            {
                raw = await reader.ReadToEndAsync();
            }

            return await DeserializeDataAsync<T>(raw);
        }

        protected async Task<T> DeserializeDataAsync<T>(string raw)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)raw;
            }

            return await _responseSerializer.DeserializeAsync<T>(raw);
        }

        protected string SerializeData<T>(T data)
        {
            if (data is string)
            {
                return data as string;
            }

            if (data == null)
            {
                return String.Empty;
            }

            return _requestSerializer.Serialize(data);
        }

        protected T DeserializeData<T>(Stream stream)
        {
            if (typeof(T) == typeof(Stream))
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)(object)ms;
            }

            string raw;
            using (var reader = new StreamReader(stream))
            {
                raw = reader.ReadToEnd();
            }

            return DeserializeData<T>(raw);
        }

        protected T DeserializeData<T>(string raw)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)raw;
            }

            return _responseSerializer.Deserialize<T>(raw);
        }
        protected string GetContentType()
        {
            return "application/json";
        }

        protected string GetAccept()
        {
            return "application/json";
        }
    }
}
