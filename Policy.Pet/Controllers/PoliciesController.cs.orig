using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Controllers
{
    [RoutePrefix("api/v1/policies")]
    public class PoliciesController : ApiController
    {
        private readonly IPolicyProvider _policyProvider;

        public PoliciesController(IPolicyProvider policyProvider, IDebugContext debugContext)
        {
            _policyProvider = policyProvider;
            _policyProvider.DebugContext = debugContext;
        }

        [Route("")]
        public async Task<IHttpActionResult> PostPetOwner(PetOwner petOwner)
        {
            var result = await _policyProvider.Enroll(petOwner);
            return Ok(result);
        }

        [Route("")]
        public async Task<IHttpActionResult> GetPetOwners()
        {
            return Ok(new List<PetOwner>
            {
                new PetOwner
                {
                    Id = 1,
                    PolicyNumber = "ETH000001",
                    PolicyDate = DateTime.UtcNow.AddHours(-12),
                    Name = "Test1",
                    Email = "test1@gmail.com",
                    CountryIsoCode = "ETH",
                },
                new PetOwner
                {
                    Id = 2,
                    Name = "Test2",
                    PolicyNumber = "USA000001",
                    PolicyDate = DateTime.UtcNow.AddHours(-6),
                    Email = "test2@gmail.com",
                    CountryIsoCode = "USA",
                }
            });
        }

        [Route("countries")]
        public async Task<IHttpActionResult> GetCountries()
        {
            return Ok(new List<Country>
            {
                new Country
                {
                    IsoCode = "USA",
                    Name = "United States"
                },
                new Country
                {
                    IsoCode = "ETH",
                    Name = "Ethiopia"
                },
                new Country
                {
                    IsoCode = "Uk",
                    Name = "United Kingdom"
                }
            });
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> GetPetOwner(int? id)
        {
            return Ok(new PetOwner
            {
                Id = 1,
                PolicyNumber = "ETH000001",
                PolicyDate = DateTime.UtcNow.AddHours(-12),
                Name = "Test1",
                Email = "test1@gmail.com",
                CountryIsoCode = "ETH",
            });
        }
    }

    public class Country
    {
        public string IsoCode { get; set; }
        public string Name { get; set; }
    }
}
