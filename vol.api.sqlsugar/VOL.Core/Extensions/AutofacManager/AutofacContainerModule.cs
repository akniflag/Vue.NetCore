using System;
using VOL.Core.Configuration;
using VOL.Core.Extensions;

namespace VOL.Core.Extensions.AutofacManager
{
    public class AutofacContainerModule
    {
        public static TService GetService<TService>()
            where TService : class
        {
            return typeof(TService).GetService() as TService;
        }
    }
}
