using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Common.Configuration;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;
using Policy.Pets.Controllers;
using Policy.Pets.Models;
using Policy.Pets.Provider;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Tests
{
    [TestFixture]
    public class PetControllerTests
    {
        protected StandardKernel Kernel { get; set; }

        private readonly IPetPolicyProvider _policyProvider;
        private readonly IDebugContext _debugContext; 
        public PetControllerTests()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IConfiguration>().To<Configuration>();
            Kernel.Bind<IDebugContext>().To<DebugContext>();
            Kernel.Bind<IPetPolicyProvider>().To<PetPolicyProvider>();

            _policyProvider = Kernel.Get<PetPolicyProvider>();
            _debugContext = Kernel.Get<IDebugContext>();
            _policyProvider.DebugContext = _debugContext;
        }

        [SetUp]
        public void Init()
        { /* ... */ }

        [TearDown]
        public void Cleanup()
        { /* ... */ }

        private static readonly string[] PetNames = { "Bobby", "户网站", "لاخلالاغ‎" };

        [Test,
         TestCaseSource("PetNames"),
         Category("PolicyTests"),
         Description("Add Pet to Policy")]
        public async Task AddToPolicyTests_PetNames(string name)
        {
            var petProviderMock = new Mock<IPetProvider>();
            petProviderMock.Setup((pp => pp.Create(It.IsAny<Pet>())))
                .Returns(Task.FromResult(
                new Pet
                {
                    Name = name,
                    BreedName = "German Shepherd",
                    PetOwnerId = 1
                }));

            var controller = GetController(petProviderMock.Object);

            var result = await controller.AddPetToPolicy(null);
            var response = await result.ExecuteAsync(new CancellationToken());
            var pet = await response.Content.ReadAsAsync<Pet>();

            pet.Name.Should().Be(name);
            pet.BreedName.Should().Be("German Shepherd");
            pet.PetOwnerId.Should().Be(1);
        }

        [Test,
         Category("PolicyTests"),
         Description("Remove Pet from Policy")]
        public async Task RemoveFromPolicyTests_Pet()
        {
            //AddPetToPolicy
            var petProviderMock = new Mock<IPetProvider>();
            petProviderMock.Setup((pp => pp.Create(It.IsAny<Pet>())))
                .Returns(Task.FromResult(
                    new Pet
                    {
                        Id = 1,
                        Name = "ForDeleteFromConroller",
                        BreedName = "Maine Coon",
                        PetOwnerId = 1
                    }));

            var controller = GetController(petProviderMock.Object);

            var result = await controller.AddPetToPolicy(null);
            var response = await result.ExecuteAsync(new CancellationToken());
            var addedPet = await response.Content.ReadAsAsync<Pet>();
            addedPet.Archived.Should().Be(false);

            //RemovePetFromPolicy
            petProviderMock = new Mock<IPetProvider>();
            petProviderMock.Setup((pp => pp.Delete(It.IsAny<int>())))
                .Returns(Task.FromResult(addedPet));

             controller = GetController(petProviderMock.Object);

             result = await controller.RemovePetFromPolicy(0);
             response = await result.ExecuteAsync(new CancellationToken());
             response.Content.Should().Be(null);
        }

        [Test,
         Category("PolicyTests"),
         Description("Transfer Pet")]
        public async Task TransferPet()
        {
            var petProviderMock = new Mock<IPetProvider>();
            petProviderMock.Setup((pp => pp.Create(It.IsAny<Pet>())))
                .Returns(Task.FromResult(
                    new Pet
                    {
                        Name = "ForTransfer",
                        BreedName = "Maine Coon",
                        PetOwnerId = 2
                    }));

            var controller = GetController(petProviderMock.Object);

            var result = await controller.AddPetToPolicy(null);
            var response = await result.ExecuteAsync(new CancellationToken());
            var pet = await response.Content.ReadAsAsync<Pet>();

            //controller.Request.SetRouteData();
            //var petsWithNewOwner = await controller.TransferPet();

            //foreach (var p in petsWithNewOwner)
            //{
            //    p.PetOwnerId.Should().Be(petOwner.Id);
            //}
        }

        private PetsController GetController(IPetProvider policyProvider)
        {
            var controller = new PetsController(policyProvider, _debugContext) { Request = new HttpRequestMessage() };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return controller;
        }
    }
}
