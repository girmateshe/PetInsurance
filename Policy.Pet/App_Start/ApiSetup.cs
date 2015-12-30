using Common.Configuration;
using Ninject;
using Policy.Pets.Provider;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets
{
    public class ApiSetup
    {
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IConfiguration>().ToConstant(new Configuration());
            kernel.Bind<IDebugContext>().To<DebugContext>();
            kernel.Bind<IPetProvider>().To<PetProvider>().InSingletonScope();
            kernel.Bind<ICountryProvider>().To<CountryProvider>().InSingletonScope();
            kernel.Bind<IBreedProvider>().To<BreedProvider>().InSingletonScope();
            kernel.Bind<IPetPolicyProvider>().To<PetPolicyProvider>().InSingletonScope();
        }
    }
}