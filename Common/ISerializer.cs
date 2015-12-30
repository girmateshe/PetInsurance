using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ISerializer
    {
        Task<string> SerializeAsync<T>(T obj);
        Task<T> DeserializeAsync<T>(string raw);
        string Serialize<T>(T obj);
        T Deserialize<T>(string raw);
    }
}
