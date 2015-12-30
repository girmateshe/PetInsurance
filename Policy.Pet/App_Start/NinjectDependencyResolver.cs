using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;

namespace Policy.Pets
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            //do nothing
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}
