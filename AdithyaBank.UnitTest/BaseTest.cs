using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.DataContext;
using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdithyaBank.BackEnd.RepoInterfaces;
using AdithyaBank.BackEnd.Repositories;
using Microsoft.AspNetCore.DataProtection;

namespace AdithyaBank.UnitTest
{
    public abstract class BaseTest
    {
        protected internal readonly AdithyaBankDatabaseContext _context;
        protected internal readonly IOptions<AppSettings> _appSettings;
        protected internal readonly ICommonRepository _commonRepository;
        protected internal readonly IDataProtector _protector;
        public BaseTest()
        {
            var contextOptions = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<AdithyaBankDatabaseContext>(), Constants.ConnectionString, b => { b.UseRowNumberForPaging(); }).Options;
            //ILoggerFactory logFactory = new LoggerFactory().AddDebug();

            //set app settings
            _appSettings = Options.Create(new AppSettings()
            {
                DatabaseConnectionString = Constants.ConnectionString,
                Organization = Constants.Organization,
                ImagesPath = Constants.ImagePath
            });

            //set db context
            _context = new AdithyaBankDatabaseContext(contextOptions);
            _commonRepository = new CommonRepository(_context,_protector);
        }
        public virtual void InitTest()
        {

        }
        [TestCleanup]
        public virtual void CleanUp() => _context.Dispose();
    }
}
