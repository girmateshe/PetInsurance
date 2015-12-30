using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common.Configuration;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{

    public class PetProvider : BasePetProvider<Pet> , IPetProvider
    {
        public PetProvider(IConfiguration configuration) : 
            base(configuration.ConnectionStrings[DatabaseType.LocalDb])
        {
            
        }

        public new async Task<Pet> GetById(int id)
        {
            var pet = await ExecuteSingle<Pet>("GetPetById",
                new List<SqlParam>
                            {
                                new SqlParam {Name = "ID", Value = id, Type = SqlDbType.Int},
                            }
            );

            return pet;
        }

        public override async Task<IEnumerable<Pet>> GetAll()
        {
            return await GetAllByPolicyId(null);
        }

        public new async Task<Pet> Create(Pet pet)
        {
            var addedPet = await ExecuteSingle<Pet>("AddPetToPolicy",
                new List<SqlParam>
                {
                    new SqlParam {Name = "PetOwnerID", Value = pet.PetOwnerId, Type = SqlDbType.Int},
                    new SqlParam {Name = "Name", Value = pet.Name, Type = SqlDbType.NVarChar, Size = 200},
                    new SqlParam {Name = "BreedName", Value = pet.BreedName, Type = SqlDbType.NVarChar, Size = 200},
                    new SqlParam {Name = "DateOfBirth", Value = pet.DateOfBirth, Type = SqlDbType.DateTime}
                }
            );

            return addedPet;
        }

        public new async Task<Pet> Delete(int id)
        {
            var removedPet = await ExecuteSingle<Pet>("RemovePetFromPolicy",
                new List<SqlParam>
                {
                    new SqlParam {Name = "ID", Value = id, Type = SqlDbType.Int},
                }
            );

            return removedPet;
        }

        public async Task<IEnumerable<Pet>> GetAllByPolicyId(int? policyId)
        {
            var pets = await ExecuteList<Pet>("GetPets",
                new List<SqlParam>
                            {
                                new SqlParam {Name = "PolicyId", Value = policyId, Type = SqlDbType.Int}
                            }
            );

            return pets.ToList();
        }

        public async Task<IList<Pet>> TransferPet(int fromOwnerId, int toOwnerId)
        {
            var pets = await ExecuteList<Pet>("TransferOwner",
                new List<SqlParam>
                {
                    new SqlParam {Name = "fromOwnerId", Value = fromOwnerId, Type = SqlDbType.Int},
                    new SqlParam {Name = "toOwnerId", Value = toOwnerId, Type = SqlDbType.Int},
                }
                );

            return pets.ToList();
        }
    }
}
