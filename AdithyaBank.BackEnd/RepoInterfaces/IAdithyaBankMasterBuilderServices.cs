using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAdithyaBankMasterBuilderServices
    {
        IServiceCollection Services { get; }
    }
}
