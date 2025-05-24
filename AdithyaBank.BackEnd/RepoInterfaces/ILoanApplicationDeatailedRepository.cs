using AdithyaBank.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.RepoInterfaces
{
    public interface ILoanApplicationDeatailedRepository
    {
        Task<ApplicationGetModel> Getappraisaldetails(long id);
    }
}
