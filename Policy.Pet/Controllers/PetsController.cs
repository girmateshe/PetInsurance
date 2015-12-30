using System.Threading.Tasks;
using System.Web.Http;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Controllers
{
    [RoutePrefix("api/v1")]
    public class PetsController : ApiController
    {
        private readonly IPetProvider _petProvider;

        public PetsController(IPetProvider petProvider, IDebugContext debugContext)
        {
            _petProvider = petProvider;
            _petProvider.DebugContext = debugContext;
        }

        [Route("pets/{id}")]
        public async Task<IHttpActionResult> GetPetById(int id)
        {
            var result = await _petProvider.GetById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Route("policies/{policyId}/pets")]
        public async Task<IHttpActionResult> GetPets(int policyId)
        {
            var result = await _petProvider.GetAllByPolicyId(policyId);
            return Ok(result);
        }

        [HttpPost, Route("pets")]
        public async Task<IHttpActionResult> AddPetToPolicy(Pet pet)
        {
            var result = await _petProvider.Create(pet);
            return Created(string.Empty, result);
        }

        [HttpPut, Route("pets/transfer/{from:int}/{to:int}")]
        public async Task<IHttpActionResult> TransferPet(int from, int to)
        {
            var result = await _petProvider.TransferPet(from, to);
            return Ok(result);
        }

        [HttpDelete, Route("pets{id}")]
        public async Task<IHttpActionResult> RemovePetFromPolicy(int id)
        {
            await _petProvider.Delete(id);
            return Ok();
        }
   }
}
