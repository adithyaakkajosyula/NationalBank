using AdithyaBank.BackEnd.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdithyaBank.BackEnd.Models
{
    public class Constants
    {
        public static string DataProtectionKey = "JFL";
        public struct RolesList
        {
            public const string Admin = "f93098f1-5e68-4348-a8de-bb237996f1fc";
            public const string HR = "e49f3fc1-e1b6-4d8f-8a95-a2da9265d89a";
        }
        public enum AppraisalTabFor
        {
            [StringValue("Appraisal")]
            Appraisal,
            [StringValue("Address")]
            Address,
            [StringValue("FamilyDetails")]
            FamilyDetails,
            [StringValue("KycDetail")]
            KycDetail,
            [StringValue("NomineeDetail")]
            NomineeDetail,
            [StringValue("CoApplicant")]
            CoApplicant,
            [StringValue("BusinessInfo")]
            BusinessInfo,
            [StringValue("SalesInfo")]
            SalesInfo,
            [StringValue("IncomeInfo")]
            IncomeInfo,
            [StringValue("OtherIncome")]
            OtherIncome,
            [StringValue("BankInfo")]
            BankInfo,
            [StringValue("LoanRequest")]
            LoanRequest,
            [StringValue("Collateral1")]
            Collateral1,
            [StringValue("Collateral2")]
            Collateral2,
            [StringValue("Collateral3")]
            Collateral3,
            [StringValue("Collateral4")]
            Collateral4,
            [StringValue("Documents")]
            Documents,
            [StringValue("Completion")]
            Completion,
            [StringValue("ForwordToCB")]
            ForwordToCB,
            [StringValue("AppraisalAnalyst")]
            AppraisalAnalyst,
            [StringValue("AppraisalLAP")]
            AppraisalLAP,
            [StringValue("Liabilities")]
            Liabilities,
            [StringValue("Televerification")]
            Televerification

        }

    }
}
