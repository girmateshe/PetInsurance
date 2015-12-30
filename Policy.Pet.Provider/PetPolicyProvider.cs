using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common.Configuration;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{

    public class PetPolicyProvider : BasePetProvider<PetOwner> , IPetPolicyProvider
    {
        public PetPolicyProvider(IConfiguration configuration) : 
            base(configuration.ConnectionStrings[DatabaseType.LocalDb])
        {
            
        }

        public async Task<PetOwner> GetById(int id)
        {
            var policy = await ExecuteSingle<PetOwner>("GetPetOwnerById",
                new List<SqlParam>
                {
                    new SqlParam {Name = "ID", Value = id, Type = SqlDbType.NVarChar, Size = 40},
                }
            );
            return policy;
        }

        public async override Task<IEnumerable<PetOwner>> GetAll()
        {
            var policy = await ExecuteList<PetOwner>("GetPetOwners");

            return policy.ToList();
        }

        public async Task<PetOwner> Create(PetOwner petOwner)
        {
            var policy = await ExecuteSingle<PetOwner>("InsertPetOwner",
                new List<SqlParam> { 
                                    new SqlParam { Name = "Name", Value = petOwner.Name, Type = SqlDbType.NVarChar, Size = 200 },
                                    new SqlParam { Name = "IsoCode", Value = petOwner.CountryIsoCode, Type = SqlDbType.Char, Size = 3},
                                    new SqlParam { Name = "Email", Value = petOwner.Email, Type = SqlDbType.NVarChar, Size = 256}
                }
            );

            return policy;
        }

        public async Task<PetOwner> Delete(int ownerId)
        {
            var policy = await ExecuteSingle<PetOwner>("DeletePetOwner",
                new List<SqlParam>
                {
                    new SqlParam {Name = "ID", Value = ownerId, Type = SqlDbType.NVarChar, Size = 40},
                }
            );

            return policy;
        }
    }
}
