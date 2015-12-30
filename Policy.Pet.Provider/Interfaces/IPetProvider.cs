using System.Collections.Generic;
using System.Threading.Tasks;
using Policy.Pets.Models;

namespace Policy.Pets.Provider.Interfaces
{
    public interface IPetProvider : IProvider<Pet>
    {
        Task<IEnumerable<Pet>> GetAllByPolicyId(int? policyId);
        Task<IList<Pet>> TransferPet(int fromOwnerId, int toOwnerId);
    }
}
