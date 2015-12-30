using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Configuration;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{
    public class BreedProvider : BasePetProvider<Breed> , IBreedProvider
    {
        public BreedProvider(IConfiguration configuration) : 
            base(configuration.ConnectionStrings[DatabaseType.LocalDb])
        {
        }

        public async override Task<IEnumerable<Breed>> GetAll()
        {
            var breeds = await ExecuteList<Breed>("GetBreeds");
            return breeds.ToList();
        }

        public Task<Breed> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Breed> Create(Breed pet)
        {
            throw new NotImplementedException();
        }

        public Task<Breed> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
