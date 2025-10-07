using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Repositories
{
    public class ComplaintsRepository:IComplaintsRepository
    {
        private readonly NationalBankDatabaseContext _context;
        public ComplaintsRepository(NationalBankDatabaseContext context)
        {

            _context = context;
        }
        public async Task<List<ComplaintsModel>> GetComplaints(int pageNumber, int pageSize)
        {
            var complaints = await _context.Complaints
                .OrderByDescending(c => c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ComplaintsModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Category = a.Category,
                    Description = a.Description,
                    Date = a.Date,
                    Status = a.Status,
                    Priority = a.Priority
                })
                .ToListAsync();

            return complaints;
        }

        public async Task<List<ComplaintsModel>> GetComplaintsByStoredProcedure(int pageNumber, int pageSize)
        {
            var complaints = await _context.ComplaintsModel.FromSqlInterpolated($"exec getcomplaints {pageNumber},{pageSize}").ToListAsync();

            return complaints;
        }

    }
}
