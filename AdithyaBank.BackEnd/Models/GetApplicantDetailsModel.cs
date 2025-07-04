using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Models
{
    public class GetApplicantDetailsModel : BaseModel
    {
        public string ApplicationName { get; set; }
        public string ApplicationFatherName { get; set; }
        public string ApplicationMotherName { get; set; }
        public DateTime ApplicationDob { get; set; }
        public char ApplicationGender { get; set; }
        public string ApplicationQualification { get; set; }
        public string ApplicationMartialStatus { get; set; }
        public string ApplicationMobile { get; set; }
        public string ApplicationEmail { get; set; }
        public long? ApplicationDocumentTypeId { get; set; }
        public decimal ApplicationRequestedAmount { get; set; }
        public IEnumerable<string> ApplicationHobbies { get; set; }
        public DateTime ApplicationRegisterDate { get; set; }
        public bool ApplicationIsAcceptedTermsAndConditions { get; set; }
        public string ApplicationAddress { get; set; }
        public long ApplicationDistrictId { get; set; }
        public long ApplicationStateId { get; set; }
        public string ApplicationCountryName { get; set; }
        public long ApplicationCountryId { get; set; }
        public IFormFile DocumentFile { get; set; }

    }
}
