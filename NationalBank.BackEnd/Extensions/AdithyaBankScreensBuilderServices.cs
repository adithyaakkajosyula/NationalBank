using NationalBank.BackEnd.Authorization;
using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Entities;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using NationalBank.BackEnd.Repositories;
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
    public static class NationalBankScreensBuilderServices
    {
        public static INationalBankMasterBuilderServices AddNationalBanksMasterServices(this INationalBankMasterBuilderServices builder)
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
