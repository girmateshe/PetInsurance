using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common.Configuration;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{
    public class CountryProvider : BasePetProvider<Country> , ICountryProvider
    {
        public CountryProvider(IConfiguration configuration) : 
            base(configuration.ConnectionStrings[DatabaseType.LocalDb])
        {
            
        }

        public async override Task<IEnumerable<Country>> GetAll()
        {
            var pets = await ExecuteList<Country>("GetCountries");

            return pets.ToList();
        }

        public Task<Country> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Country> Create(Country pet)
        {
            throw new NotImplementedException();
        }

        public Task<Country> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
