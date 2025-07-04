using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class LoanRepayment:BaseEntity
    {
        public long LoanId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? RecDate { get; set; }
        public long PaymentTypeId { get; set; }
        public long StaffId { get; set; }
        public int InstalmentNo { get; set; }
        public decimal PrinDue { get; set; }
        public decimal IntDue { get; set; }
        public decimal PrinColl { get; set; }
        public decimal IntColl { get; set; }
        public decimal Prepaid { get; set; }
        public decimal PrepaidCharges { get; set; }
        public decimal PenalInterestDue { get; set; }
        public decimal PenalInterestColl { get; set; }
        public decimal PastDuePrinColl { get; set; }
        public decimal PastDueIntColl { get; set; }
        public decimal Bounce_Due { get; set; }
        public decimal Bounce_IGST_Due { get; set; }
        public decimal Bounce_CGST_Due { get; set; }
        public decimal Bounce_UGST_Due { get; set; }
        public decimal Bounce_SGST_Due { get; set; }
        public decimal Bounce_Collected { get; set; }
        public decimal Bounce_IGST_Collected { get; set; }
        public decimal Bounce_CGST_Collected { get; set; }
        public decimal Bounce_UGST_Collected { get; set; }
        public decimal Bounce_SGST_Collected { get; set; }
        public decimal OtherCharges_Due { get; set; }
        public decimal OtherCharges_IGST_Due { get; set; }
        public decimal OtherCharges_CGST_Due { get; set; }
        public decimal OtherCharges_UGST_Due { get; set; }
        public decimal OtherCharges_SGST_Due { get; set; }
        public decimal OtherCharges_Collected { get; set; }
        public decimal OtherCharges_IGST_Collected { get; set; }
        public decimal OtherCharges_CGST_Collected { get; set; }
        public decimal OtherCharges_UGST_Collected { get; set; }
        public decimal OtherCharges_SGST_Collected { get; set; }
        public byte RowState { get; set; }
        public decimal PreCloseChargesGST { get; set; }
        public decimal? PenalInterestCollGST { get; set; }
        public Loan Loan { get; set; }
    }
}
