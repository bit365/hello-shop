using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloShop.ServiceDefaults.Infrastructure
{
    public interface IDataSeedingProvider
    {
        int Order => default;

        Task SeedingAsync(IServiceProvider serviceProvider);
    }
}
