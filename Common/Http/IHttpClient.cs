using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
    public interface IHttpClient
    {
        HttpResponse<T> Get<T>(Uri url);
        HttpResponse<T> Post<T>(Uri url, T data);
        HttpResponse<T> Put<T>(Uri url, T data);
        HttpResponse<T> Delete<T>(Uri url);

        HttpResponse<TOut> Post<TIn, TOut>(Uri url, TIn data);
        HttpResponse<TOut> Put<TIn, TOut>(Uri url, TIn data);
        HttpStatusCode Delete(Uri url);
    
        Task<HttpResponse<T>> GetAsync<T>(Uri url);
        Task<HttpResponse<T>> PostAsync<T>(Uri url, T data);
        Task<HttpResponse<T>> PutAsync<T>(Uri url, T data);
        Task<HttpResponse<T>> DeleteAsync<T>(Uri url);

        Task<HttpResponse<TOut>> PostAsync<TIn, TOut>(Uri url, TIn data);
        Task<HttpResponse<TOut>> PutAsync<TIn, TOut>(Uri url, TIn data);
        Task<HttpStatusCode> DeleteAsync(Uri url);
    }
}
