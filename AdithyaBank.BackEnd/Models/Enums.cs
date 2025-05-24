

using AdithyaBank.BackEnd.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AdithyaBank.BackEnd.Models
{
    public class Enums
    {
        public enum AppraisalListFor
        {
            AppraisalEntry,
            AppraisalList,
            AppraisalApproval,
            AppraisalAnalyst,
            AppraisalAdmin,
            BussinessVerification,
            ForwardToCB,
            CBEnquiry,
            CBUploads,
            TeleverificationQuestions,
            Televerification,
            Compare,
            Score,
            Legal,
            CBEnquiryUpload,
            CreditComments,
            CreditCommitee,
            ProvisionalAcceptance,
            FinalApproval,
            DisbursementRequest,
            OperationsApprove,
            CreditApprove,
            TreasuryApprove,
            Disbursement
        }
        public enum RoleRequired
        {
            [DisplayName("f93098f1-5e68-4348-a8de-bb237996f1fc")]
            Admin,
            [DisplayName("e49f3fc1-e1b6-4d8f-8a95-a2da9265d89a")]
            HR  
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class DisplayNameAttribute : Attribute
        {
            public string DisplayName { get; }

            public DisplayNameAttribute(string displayName)
            {
                DisplayName = displayName;
            }
        }
        public enum ApplicationQualifications : byte
        {
            [Display(Name = "SSC")]
            SSC = 1,
            [Display(Name = "INTER")]
            INTER = 2,
            [Display(Name = "DEGREE")]
            DEGREE = 3,
            [Display(Name = "PG")]
            PG = 4
  
        }
        public enum ApplicantHobbies : byte
        {
            [Display(Name ="Running")]
            Running = 1,
            [Display(Name ="Walking")]
            Walking = 2,
            [Display(Name ="Dancing")]
            Dancing = 3,
        }
        public enum AppraisalType
        {
            [StringValue("N")]
            New = 1,
            [StringValue("R")]
            Renewal = 2,
            [StringValue("T")]
            TopUp = 3,
            [StringValue("S")]
            Supplementory = 4
        }
        public enum Gender
        {
            [StringValue("M")]
            [Display(Name = "Male")]
            M = 1,
            [StringValue("F")]
            [Display(Name = "Female")]
            F = 2,
        }
    }
}
