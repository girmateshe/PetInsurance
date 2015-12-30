using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Configuration;
using FluentAssertions;
using Ninject;
using NUnit.Framework;
using Policy.Pets.Models;
using Policy.Pets.Provider;
using Policy.Pets.Provider.Interfaces;
using System.Collections.Generic;

namespace Policy.Pets.Tests
{
    [TestFixture]
    public class PetProviderTests
    {
        protected StandardKernel Kernel { get; set; }

        private readonly IPetProvider _petProvider;
        private readonly IPetPolicyProvider _petPolicyProvider;

        public PetProviderTests()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IConfiguration>().To<Configuration>();
            Kernel.Bind<IDebugContext>().To<DebugContext>();
            Kernel.Bind<IPetProvider>().To<PetProvider>();
            Kernel.Bind<IPetPolicyProvider>().To<PetPolicyProvider>();

            _petProvider = Kernel.Get<PetProvider>();
            _petPolicyProvider = Kernel.Get<PetPolicyProvider>();

            _petProvider.DebugContext = Kernel.Get<DebugContext>();
            _petPolicyProvider.DebugContext = Kernel.Get<DebugContext>();
        }

        [SetUp]
        public void Init()
        {
            /* ... */
        }

        [TearDown]
        public void Cleanup()
        {
            /* ... */
        }

        [Test,
         Category("PolicyTests"),
         Description("Get Pets")]
        public async Task GetPetsTests()
        {
            var pets = await _petProvider.GetAll();
        }

        private static readonly string[] Names = {"Teshe", "户网站", "محمد‎"};
        private static readonly string[] IsoCodes = {"ETH", "USA", "SAU"};
        private static readonly string[] Emails = {"test1@gmail.com", "户网站@gmail.com", "محمد@gmail.com"};

        private static readonly string[] PetNames = {"Bobby", "户网站", "لاخلالاغ‎"};

        [Test,
         TestCaseSource("PetNames"),
         Category("PolicyTests"),
         Description("Add Pet to Policy")]
        public async Task AddToPolicyTests_PetNames(string name)
        {
            var pet = await _petProvider.Create(
                new Pet
                {
                    Name = name,
                    BreedName = "German Shepherd",
                    PetOwnerId = 1,
                    DateOfBirth = DateTime.UtcNow.AddYears(-1)
                });

            pet.Name.Should().Be(name);
            pet.BreedName.Should().Be("German Shepherd");
            pet.PetOwnerId.Should().Be(1);
        }

        [Test,
         Category("PolicyTests"),
         Description("Remove Pet from Policy")]
        public async Task RemoveFromPolicyTests_Pet()
        {
            Pet deletedPet;
            var pets = await _petProvider.GetAll();

            var firstPet = pets.FirstOrDefault();
            if (firstPet != null)
            {
                deletedPet = await _petProvider.Delete(firstPet.Id);
            }
            else
            {
                var pet = await _petProvider.Create(
                    new Pet
                    {
                        Name = "ForDeleteFromProvider",
                        BreedName = "German Shepherd",
                        PetOwnerId = 1
                    });
                pet.Archived.Should().Be(false);

                deletedPet = await _petProvider.Delete(pet.Id);
            }
            deletedPet.Archived.Should().Be(true);
        }

        [Test,
         Category("PolicyTests"),
         Description("Transfer Pet")]
        public async Task TransferPet()
        {
            var pets = await _petProvider.GetAll();
            var petOwners = await _petPolicyProvider.GetAll();

            var pet = pets.FirstOrDefault();
            var petOwner = petOwners.FirstOrDefault();

            if (pet != null && petOwner != null)
            {
                var petsWithNewOwner = await _petProvider.TransferPet(pet.PetOwnerId, petOwner.Id);
                foreach (var p in petsWithNewOwner)
                {
                    p.PetOwnerId.Should().Be(petOwner.Id);
                }
            }
        }
    }
}
