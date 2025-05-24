using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdithyaBank.BackEnd.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdithyaBankBuilderServicesExtensions
    {
        public static IAdithyaBankMasterBuilderServices AddAdithyaBankMasterBuilderServices(this IServiceCollection services) => new AdithyaBankMasterBuilderServices(services);
        public static IAdithyaBankMasterBuilderServices AddAdithyamainServices(this IServiceCollection services)
        {

            var builder = services.AddAdithyaBankMasterBuilderServices();
            builder.AddAdithyaBanksMasterServices();
            return builder;
        }
    }
}
