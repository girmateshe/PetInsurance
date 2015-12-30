using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Configuration;
using Common.Http;
using Common.Json;
using FluentAssertions;
using Ninject;
using NUnit.Framework;
using Policy.Pets.Models;
using Newtonsoft.Json;

namespace Policy.Pets.IntegrationTests
{
    [TestFixture]
    public class PetTests
    {
        protected StandardKernel Kernel { get; set; }

        private readonly IConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly string _petsBaseUrl;
        private readonly string _policiesBaseUrl;

        public PetTests()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IConfiguration>().To<Configuration>();
            Kernel.Bind<ISerializer>().To<Common.Json.JsonSerializer>();
            Kernel.Bind<IHttpClient>().To<HttpClient>();
            Kernel.Bind<JsonSerializerSettings>().To<JsonSerializerSettings>();


            _configuration = Kernel.Get<IConfiguration>();
            _httpClient = Kernel.Get<IHttpClient>();

            _petsBaseUrl = _configuration.RootRestApiUrl + "/pets";
            _policiesBaseUrl = _configuration.RootRestApiUrl + "/policies";
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

        private static readonly string[] PetNames = {"Bobby", "户网站", "لاخلالاغ‎"};

        [Test,
         TestCaseSource("PetNames"),
         Category("PolicyTests"),
         Description("Add Pet to Policy")]
        public async Task AddToPolicyTests_PetNames(string name)
        {
            var result = await _httpClient.PostAsync<Pet, Pet>
                (new Uri(_petsBaseUrl),
                    new Pet
                    {
                        Name = name,
                        BreedName = "German Shepherd",
                        PetOwnerId = 2
                    });

            var pet = result.Content;

            pet.Name.Should().Be(name);
            pet.BreedName.Should().Be("German Shepherd");
            pet.PetOwnerId.Should().Be(2);
        }

        [Test,
         Category("PolicyTests"),
         Description("Remove Pet from Policy")]
        public async Task RemoveFromPolicyTests_Pet()
        {
            var addPetToPolicyResult = await _httpClient.PostAsync<Pet, Pet>
                (new Uri(_petsBaseUrl),
                    new Pet
                    {
                        Name = "ForDeleteFromIntegration",
                        BreedName = "Maltipoo",
                        PetOwnerId = 3
                    });

            var addedPet = addPetToPolicyResult.Content;

            addPetToPolicyResult.StatusCode.Should().Be(HttpStatusCode.Created);
            addedPet.Name.Should().Be("ForDeleteFromIntegration");
            addedPet.BreedName.Should().Be("Maltipoo");
            addedPet.PetOwnerId.Should().Be(3);
            addedPet.Archived.Should().Be(false);

            var removePetFromPolicyResult = await _httpClient.DeleteAsync<Pet>
                                                    (new Uri(_petsBaseUrl + "/" + addedPet.Id));

            removePetFromPolicyResult.StatusCode.Should().Be(200);
            removePetFromPolicyResult.Content.Should().Be(null);
        }

        [Test,
         Category("PolicyTests"),
         Description("Transfer PetOwner")]
        public async Task TransferOwnerTest()
        {
            //Enroll PetOwner One
            var enrollOneResult = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_policiesBaseUrl),
                    new PetOwner
                    {
                        Name = "ForTransferOne",
                        CountryIsoCode = "ETH",
                        Email = "ForTransferOne@yahoo.com"
                    });

            var enrollOnePolicy = enrollOneResult.Content;

            enrollOneResult.StatusCode.Should().Be(HttpStatusCode.Created);
            enrollOnePolicy.Name.Should().Be("ForTransferOne");
            enrollOnePolicy.CountryIsoCode.Should().Be("ETH");
            enrollOnePolicy.Email.Should().Be("ForTransferOne@yahoo.com");
            enrollOnePolicy.Archived.Should().Be(false);

            //Enroll PetOwner Two

            var enrollTwoResult = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_policiesBaseUrl),
                    new PetOwner
                    {
                        Name = "ForTransferTwo",
                        CountryIsoCode = "USA",
                        Email = "ForTransferTwo@yahoo.com"
                    });

            var enrollTwoPolicy = enrollTwoResult.Content;

            enrollTwoPolicy.Name.Should().Be("ForTransferTwo");
            enrollTwoPolicy.CountryIsoCode.Should().Be("USA");
            enrollTwoPolicy.Email.Should().Be("ForTransferTwo@yahoo.com");
            enrollTwoPolicy.Archived.Should().Be(false);

            //Add pet into Owner one policy
            var result = await _httpClient.PostAsync<Pet, Pet>
                (new Uri(_petsBaseUrl),
                    new Pet
                    {
                        Name = "addPetForTransferTest",
                        BreedName = "German Shepherd",
                        PetOwnerId = enrollOnePolicy.Id
                    });

            var pet = result.Content;

            pet.Name.Should().Be("addPetForTransferTest");
            pet.BreedName.Should().Be("German Shepherd");
            pet.PetOwnerId.Should().Be(enrollOnePolicy.Id);

            //Transfer from petowner One to petowner two

            var transfer = await _httpClient.PutAsync<PetOwner, IList<Pet>>(new Uri(_petsBaseUrl + "/transfer/" + enrollOnePolicy.Id + "/" + enrollTwoPolicy.Id), null);
            var pets = transfer.Content;
            transfer.StatusCode.Should().Be(HttpStatusCode.OK);
            foreach (var p in pets)
            {
                p.PetOwnerId.Should().Be(enrollTwoPolicy.Id);
            }
        }
    }

}
