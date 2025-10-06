using NationalBank.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.RepoInterfaces
{
    public interface IComplaintsRepository
    {
        Task<List<ComplaintsModel>> GetComplaints(int pageNumber, int pageSize);
    }
}
