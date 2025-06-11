using AdithyaBank.BackEnd.DataContext;
using AdithyaBank.BackEnd.Entities;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdithyaBank.BackEnd.Repositories
{
    public class LoanApplicationDeatailedRepository:ILoanApplicationDeatailedRepository
    {
        private readonly AdithyaBankDatabaseContext _context;
        private readonly IOptions<AppSettings> _options;
        private readonly ICommonRepository _commonRepository;
        public LoanApplicationDeatailedRepository(AdithyaBankDatabaseContext context,IOptions<AppSettings> options, ICommonRepository commonRepository)
        {
            _context = context; 
            _options = options; 
            _commonRepository = commonRepository;       
        }
        public async Task<ApplicationGetModel> Getappraisaldetails(long id)
        {
            var result = await _context.ApplicationRegister.Where(a => a.Rowstate < 3 && a.Id == id)
                .Select(a => new ApplicationGetModel() { 
                    Id = a.Id,  
                    ApplicationName = a.ApplicationName,                     
                }).FirstOrDefaultAsync();
            return result;
        }
    }

    
}
