using System.Collections.Generic;
using System.Threading.Tasks;
using Policy.Pets.Models;

namespace Policy.Pets.Provider.Interfaces
{
    public interface IProvider<T> where T : Model
    {
        IDebugContext DebugContext { get; set; }
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T petOwner);
        Task<T> Delete(int ownerId);

    }
}
