using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.DataContext;
using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBank.BackEnd.RepoInterfaces;
using NationalBank.BackEnd.Repositories;
using Microsoft.AspNetCore.DataProtection;

namespace NationalBank.UnitTest
{
    public abstract class BaseTest
    {
        protected internal readonly NationalBankDatabaseContext _context;
        protected internal readonly IOptions<AppSettings> _appSettings;
        protected internal readonly ICommonRepository _commonRepository;
        protected internal readonly IDataProtector _protector;
        public BaseTest()
        {
            var contextOptions = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<NationalBankDatabaseContext>(), Constants.ConnectionString, b => { b.UseRowNumberForPaging(); }).Options;
            //ILoggerFactory logFactory = new LoggerFactory().AddDebug();

            //set app settings
            _appSettings = Options.Create(new AppSettings()
            {
                DatabaseConnectionString = Constants.ConnectionString,
                Organization = Constants.Organization,
                ImagesPath = Constants.ImagePath
            });

            //set db context
            _context = new NationalBankDatabaseContext(contextOptions);
            _commonRepository = new CommonRepository(_context,_protector);
        }
        public virtual void InitTest()
        {

        }
        [TestCleanup]
        public virtual void CleanUp() => _context.Dispose();
    }
}
