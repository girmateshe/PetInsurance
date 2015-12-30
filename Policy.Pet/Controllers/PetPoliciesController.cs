using System.Threading.Tasks;
using System.Web.Http;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Controllers
{
    [Route("api/v1/policies/{id?}")]
    public class PetPoliciesController : ApiController
    {
        private readonly IPetPolicyProvider _policyProvider;

        public PetPoliciesController(IPetPolicyProvider policyProvider, IDebugContext debugContext)
        {
            _policyProvider = policyProvider;
            _policyProvider.DebugContext = debugContext;
        }

        public async Task<IHttpActionResult> GetPetOwnerById(int id)
        {
            var result = await _policyProvider.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetPetOwners()
        {
            var result = await _policyProvider.GetAll();
            return Ok(result);
        }

        public async Task<IHttpActionResult> PostPetOwner(PetOwner petOwner)
        {
            var result = await _policyProvider.Create(petOwner);
            return Created(string.Empty, result);
        }

        public async Task<IHttpActionResult> DeletePetOwner(int id)
        {
            var result = await _policyProvider.Delete(id);
            return Ok(result);
        }
    }
}
