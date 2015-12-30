using System.Threading.Tasks;
using System.Web.Http;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Controllers
{
    [Route("api/v1/countries")]
    public class CountriesController : ApiController
    {
        private readonly ICountryProvider _countryProvider;

        public CountriesController(ICountryProvider countryProvider, IDebugContext debugContext)
        {
            _countryProvider = countryProvider;
            _countryProvider.DebugContext = debugContext;
        }

        public async Task<IHttpActionResult> GetCountries()
        {
            return Ok(await _countryProvider.GetAll());
        }
   }
}
