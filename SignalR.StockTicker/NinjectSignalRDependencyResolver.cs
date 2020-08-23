using Microsoft.AspNet.SignalR;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.StockTicker
{
    public class NinjectSignalRDependencyResolver : DefaultDependencyResolver
    {
        // Ninject container is nomenclatured as Kernel by Ninject.
        private readonly IKernel _kernel;

        public NinjectSignalRDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        // Create single instance of a Type.
        public override object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}