using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Configuration
{
     class AdithyaBankMasterBuilderServices : IAdithyaBankMasterBuilderServices
    {
        public IServiceCollection Services { get; }
        public AdithyaBankMasterBuilderServices(IServiceCollection services) => Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}
