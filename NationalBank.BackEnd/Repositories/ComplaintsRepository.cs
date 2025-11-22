using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Entities;
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
        public async Task<BaseResultModel> AddComplaint(ComplaintsModel model)
        {
            var result = await _context.Complaints.FindAsync(model.Id);
            if (result == null) 
            {
                result = new Complaints();

                result.UserId = 1;
                result.Category = model.Category;   
                result.Description = model.Description;
                result.Priority = model.Priority;
                
            }

            if (result.Id == 0)
            {
                result.Rowstate = 1;
                await _context.Complaints.AddAsync(result);      
            }
            else
            {
                result.Rowstate = 2;
                _context.Complaints.Update(result); 
            }
            await _context.SaveChangesAsync();

            return new BaseResultModel() { IsSuccess = true , Message = "Saved SucessFully"}; 
        }
    }
}
