using NationalBank.BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class ApplicationRegister : BaseEntity
    {
        public string ApplicationName { get; set; }
        public string Applicationfathername { get; set; }
        public string Applicationmothername { get; set; }
        public DateTime Applicationdob { get; set; }
        public char Applicationgender { get; set; }
        public string Applicationqualification { get; set; }
        public string ApplicationMartialStatus { get; set; }
        public string Applicationmobile { get; set; }
        public string Applicationemail { get; set; }
        public decimal ApplicationRequestedAmount { get; set; }
        public string ApplicationHobbies { get; set; }
        public DateTime ApplicationRegisterdate { get; set; }
        public bool ApplicationIsAcceptedTermsandConditions { get; set; }
        public string ApplicationAddress { get; set; }
        public long ApplicationDistrictId { get; set; }
        public long ApplicationStateId { get; set; }
        public long ApplicationCountryId { get; set; }
        public bool? ApplicationIsApproved { get; set; }
        public long? ApplicationApprovedBy { get; set; }
        public DateTime? ApplicationApprovedOn { get; set; }
        public decimal? ApplicationApprovedAmount { get; set; }
        public bool ApplicationStatus { get; set; }
        public ApplicationDocumentUploads ApplicationDocumentUploads { get; set; }
        public Countries Countries { get; set; }
        public States States { get; set; }
        public Districts Districts { get; set; }
    }
}
