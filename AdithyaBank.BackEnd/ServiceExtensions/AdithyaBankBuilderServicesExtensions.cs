using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBank.BackEnd.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NationalBankBuilderServicesExtensions
    {
        public static INationalBankMasterBuilderServices AddNationalBankMasterBuilderServices(this IServiceCollection services) => new NationalBankMasterBuilderServices(services);
        public static INationalBankMasterBuilderServices AddAdithyamainServices(this IServiceCollection services)
        {

            var builder = services.AddNationalBankMasterBuilderServices();
            builder.AddNationalBanksMasterServices();
            return builder;
        }
    }
}
