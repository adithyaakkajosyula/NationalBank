using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class Loan : BaseEntity
    {
        public Loan()
        {
            LoanRepayment = new List<LoanRepayment>();
        }
        public long ApplicationId { get; set; }
        public long ProductId { get; set; }
        public string LoanAccountNumber { get; set; }
        public DateTime LoanDate { get; set; }
        public double LoanAmount { get; set; }
        public decimal Interest { get; set; }
        public string Frequency { get; set; }
        public DateTime? LoanClosedate { get; set; }
        public byte RowState { get; set; }
        public ICollection<LoanRepayment> LoanRepayment { get; set; }
    }
}
