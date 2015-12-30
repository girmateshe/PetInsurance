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
    public class PetPolicyProviderTests
    {
        protected StandardKernel Kernel { get; set; }

        private readonly IPetPolicyProvider _petPolicyProvider;

        public PetPolicyProviderTests()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<IConfiguration>().To<Configuration>();
            Kernel.Bind<IDebugContext>().To<DebugContext>();
            Kernel.Bind<IPetPolicyProvider>().To<PetPolicyProvider>();

            _petPolicyProvider = Kernel.Get<PetPolicyProvider>();
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
         Description("Get PetOwner")]
        public async Task GetPetOwnerTests()
        {
            var petOwners = await _petPolicyProvider.GetAll();
        }

        private static readonly string[] Names = {"Teshe", "户网站", "محمد‎"};
        private static readonly string[] IsoCodes = {"ETH", "USA", "SAU"};
        private static readonly string[] Emails = {"test1@gmail.com", "户网站@gmail.com", "محمد@gmail.com"};

        [Test,
         TestCaseSource("Names"),
         Category("PolicyTests"),
         Description("Test owner names")]
        public async Task EnrollPolicyTests_OwnerNames(string name)
        {
            var policy = await _petPolicyProvider.Create(
                new PetOwner
                {
                    Name = name,
                    CountryIsoCode = "USA",
                    Email = "mytest@email.com"
                });

            policy.Name.Should().Be(name);
            policy.CountryIsoCode.Should().Be("USA");
            policy.Email.Should().Be("mytest@email.com");
        }

        [Test,
         TestCaseSource("IsoCodes"),
         Category("PolicyTests"),
         Description("Test country")]
        public async Task EnrollPolicyTests_Country(string isoCode)
        {
            var policy = await _petPolicyProvider.Create(
                new PetOwner
                {
                    Name = "Test",
                    CountryIsoCode = isoCode,
                    Email = "mytest@email.com"
                });

            policy.Name.Should().Be("Test");
            policy.CountryIsoCode.Should().Be(isoCode);
            policy.Email.Should().Be("mytest@email.com");
        }

        [Test,
         TestCaseSource("Emails"),
         Category("PolicyTests"),
         Description("Test Emails")]
        public async Task EnrollPolicyTests_Emails(string email)
        {
            var policy = await _petPolicyProvider.Create(
                new PetOwner
                {
                    Name = "Test",
                    CountryIsoCode = "USA",
                    Email = email
                });

            policy.Name.Should().Be("Test");
            policy.CountryIsoCode.Should().Be("USA");
            policy.Email.Should().Be(email);
        }

        [Test,
         Category("PolicyTests"),
         Description("Cancel PetOwner")]
        public async Task CancelPolicyTest()
        {
            PetOwner deletedPetOwner;
            var policy = await _petPolicyProvider.GetAll();
            var firstPolicy = policy.FirstOrDefault();
            if (firstPolicy != null)
            {
                deletedPetOwner = await _petPolicyProvider.Delete(firstPolicy.Id);
            }
            else
            {
                var petOwner = await _petPolicyProvider.Create(
                    new PetOwner
                    {
                        Name = "Test",
                        CountryIsoCode = "USA",
                        Email = "test@test"
                    });
                deletedPetOwner = await _petPolicyProvider.Delete(petOwner.Id);
            }
            deletedPetOwner.Archived.Should().Be(true);
        }
    }
}
