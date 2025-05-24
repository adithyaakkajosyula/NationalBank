using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.DataContext;
using AdithyaBank.BackEnd.Entities;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using AdithyaBank.BackEnd.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdithyaBankScreensBuilderServices
    {
        public static IAdithyaBankMasterBuilderServices AddAdithyaBanksMasterServices(this IAdithyaBankMasterBuilderServices builder)
        {
           // builder.Services.AddScoped<IApplicationRegisterRepository, ApplicationRegisterRepository>();
            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICommonRepository,CommonRepository>();
            builder.Services.AddScoped<IApplicationRegisterRepository,ApplicationRegisterRepository>();
            return builder;
        }
    }
}
