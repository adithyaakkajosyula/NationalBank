using Microsoft.EntityFrameworkCore;
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
        public async Task<PagedResult<ComplaintsModel>> GetCommplaints(int pageNumber,int pageSize)
        {
            var query = _context.Complaints;

            var totalCount = await query.CountAsync();

            var complaints = await query
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
                    Priority = a.Priority,
                })
                .ToListAsync();

            return new PagedResult<ComplaintsModel>
            {
                Items = complaints,
                TotalCount = totalCount
            };
        }
    }
}
