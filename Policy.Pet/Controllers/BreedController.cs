using System.Threading.Tasks;
using System.Web.Http;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Controllers
{
    [Route("api/v1/breeds")]
    public class BreedController : ApiController
    {
        private readonly IBreedProvider _breedProvider;

        public BreedController(IBreedProvider breedProvider, IDebugContext debugContext)
        {
            _breedProvider = breedProvider;
            _breedProvider.DebugContext = debugContext;
        }

        public async Task<IHttpActionResult> GetCountries()
        {
            return Ok(await _breedProvider.GetAll());
        }
   }
}
