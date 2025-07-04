using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Configuration
{
     class NationalBankMasterBuilderServices : INationalBankMasterBuilderServices
    {
        public IServiceCollection Services { get; }
        public NationalBankMasterBuilderServices(IServiceCollection services) => Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}
