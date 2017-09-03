using System;

namespace CafeLib.Services
{
    public class ServiceBase : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
