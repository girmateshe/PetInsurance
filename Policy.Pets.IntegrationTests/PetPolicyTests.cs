using System;
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
    public class PetPolicyTests
    {
        protected StandardKernel Kernel { get; set; }

        private readonly IConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly string _baseUrl;

        public PetPolicyTests()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IConfiguration>().To<Configuration>();
            Kernel.Bind<ISerializer>().To<Common.Json.JsonSerializer>();
            Kernel.Bind<IHttpClient>().To<HttpClient>();
            Kernel.Bind<JsonSerializerSettings>().To<JsonSerializerSettings>();


            _configuration = Kernel.Get<IConfiguration>();
            _httpClient = Kernel.Get<IHttpClient>();

            _baseUrl = _configuration.RootRestApiUrl + "/policies";
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
         Description("Test Get Pet Owners")]
        public async Task GetPetOwners()
        {
            var result = await _httpClient.GetAsync<IEnumerable<PetOwner>>(new Uri(_baseUrl));
            var petOwners = result.Content;
        }

        [Test,
         Category("PolicyTests"),
         Description("Get Pets")]
        public async Task GetPetsTests()
        {
            var result = await _httpClient.GetAsync<IEnumerable<PetOwner>>(new Uri(_baseUrl));
            var pets = result.Content;
        }

        private static readonly string[] Names = {"Teshe", "户网站", "Mike"};
        private static readonly string[] IsoCodes = {"ETH", "USA"};
        private static readonly string[] Emails = {"test1@gmail.com", "户网站@gmail.com", "test2@yahoo.com"};

        [Test,
         TestCaseSource("Names"),
         Category("PolicyTests"),
         Description("Test owner names")]
        public async Task AddPolicyTests_OwnerNames(string name)
        {
            var result = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_baseUrl),
                    new PetOwner
                    {
                        Name = name,
                        CountryIsoCode = "USA",
                        Email = "mytest@email.com"
                    });

            var policy = result.Content;
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            policy.Name.Should().Be(name);
            policy.CountryIsoCode.Should().Be("USA");
            policy.Email.Should().Be("mytest@email.com");
        }

        [Test,
         TestCaseSource("IsoCodes"),
         Category("PolicyTests"),
         Description("Test country")]
        public async Task AddPolicyTests_Country(string isoCode)
        {
            var result = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_baseUrl),
                    new PetOwner
                    {
                        Name = "Test",
                        CountryIsoCode = isoCode,
                        Email = "mytest@email.com"
                    });

            var policy = result.Content;

            result.StatusCode.Should().Be(HttpStatusCode.Created);
            policy.Name.Should().Be("Test");
            policy.CountryIsoCode.Should().Be(isoCode);
            policy.Email.Should().Be("mytest@email.com");
        }

        [Test,
         TestCaseSource("Emails"),
         Category("PolicyTests"),
         Description("Test Emails")]
        public async Task AddPolicyTests_Emails(string email)
        {
            var result = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_baseUrl),
                    new PetOwner
                    {
                        Name = "Test",
                        CountryIsoCode = "USA",
                        Email = email
                    });

            var policy = result.Content;

            result.StatusCode.Should().Be(HttpStatusCode.Created);
            policy.Name.Should().Be("Test");
            policy.CountryIsoCode.Should().Be("USA");
            policy.Email.Should().Be(email);
        }

        [Test,
         Category("PolicyTests"),
         Description("Delete PetOwner")]
        public async Task DeletePetOwnerTests()
        {
            var enrollResult = await _httpClient.PostAsync<PetOwner, PetOwner>
                (new Uri(_baseUrl),
                    new PetOwner
                    {
                        Name = "ForDeleteFromIntegration",
                        CountryIsoCode = "ETH",
                        Email = "Integration@yahoo.com"
                    });

            var enrollPolicy = enrollResult.Content;

            enrollPolicy.Name.Should().Be("ForDeleteFromIntegration");
            enrollPolicy.CountryIsoCode.Should().Be("ETH");
            enrollPolicy.Email.Should().Be("Integration@yahoo.com");
            enrollPolicy.Archived.Should().Be(false);

            var cancelResult = await _httpClient.DeleteAsync<PetOwner>
                                    (new Uri(_baseUrl + "/" + enrollPolicy.Id));

            var cancelPolicy = cancelResult.Content;
            
            cancelPolicy.Name.Should().Be("ForDeleteFromIntegration");
            cancelPolicy.CountryIsoCode.Should().Be("ETH");
            cancelPolicy.Email.Should().Be("Integration@yahoo.com");
            cancelPolicy.Archived.Should().Be(true);
        }
    }

}
