using NationalBank.BackEnd.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NationalBank.BackEnd.Extensions.CheckBoxRequired;

namespace NationalBank.BackEnd.Models
{

    public class ApplicationRegisterModel:BaseModel
    {
        [Required]
        public string ApplicationName { get; set; }
        [Required]
        public string ApplicationFatherName { get; set; }
        [Required]
        public string ApplicationMotherName { get; set; }
        [Required]
        public DateTime ApplicationDob { get; set; }
        [Required]
        public char ApplicationGender { get; set; }
        [Required]
        public string ApplicationQualification { get; set; }
        [Required]
        public string ApplicationMartialStatus { get; set; }
        [Required]
        public string ApplicationMobile { get; set; }
        [Required]
        public string ApplicationEmail { get; set; }
        public long? ApplicationDocumentTypeId { get; set; }
        [Required]
        public decimal ApplicationRequestedAmount { get; set; }
        [Required]
        public IEnumerable<string> ApplicationHobbies { get; set; }
        [Required]
        public DateTime ApplicationRegisterDate { get; set; }
        [Required]
        public bool ApplicationIsAcceptedTermsAndConditions { get; set; }
        [Required]
        public string ApplicationAddress { get; set; }
        [Required]
        public long ApplicationDistrictId { get; set; }
        [Required]
        public long ApplicationStateId { get; set; }
        public string ApplicationCountryName { get; set; }
        [Required]
        public long ApplicationCountryId { get; set; }
        public IFormFile DocumentFile { get; set; }
        public string EncryptId { get; set; }
        public string EncryptScreenType { get; set; }
        public IList<IdNameModel> ApplicantDocumentTypes { get; set; }
        public IList<IdNameModel> CountryTypes { get; set; }
        public IList<IdNameModel> StateTypes { get; set; }
        public IList<IdNameModel> DistrictTypes { get; set; }
        public ApplicationDocumentUploadModel ApplicationDocumentUploadModel { get; set; }
    }
    public class ApplicationDocumentUploadModel
    {
        public long DocumnentUploadId { get; set; }
        public string DocumentName { get; set; }
        public long DocumentTypeId { get; set; }
    }
    public class ApplicationGetModel : BaseModel
    {
        public string ApplicationName { get; set; }
        public LoanDetails LoanDetails { get; set; }

    }


    public class AppraisalInitialInfo
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
    }
    public class AppriasalMenuTabModel
    {
        public string AppraisalId { get; set; }
        public string ScreenType { get; set; }

        public AppraisalTabFor TabFor { get; set; }
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
    public class ApplicationTabDisplayModel
    {
        public bool Appraisal { get; set; } = true;
        public bool Address { get; set; } = true;
        public bool FamilyDetails { get; set; } = true;
        public bool KycDetail { get; set; } = true;
        public bool NomineeDetail { get; set; } = true;
        public bool CoApplicant { get; set; } = true;
        public bool BusinessInfo { get; set; } = true;
        public bool SalesInfo { get; set; } = true;
        public bool IncomeInfo { get; set; } = true;
        public bool OtherIncome { get; set; } = true;
        public bool BankInfo { get; set; } = true;
        public bool Collateral { get; set; } = true;
        public bool LoanRequest { get; set; } = true;
        public bool Documents { get; set; } = true;
        public bool Completion { get; set; } = true;
        public bool ForwordToCB { get; set; } = true;
        public bool AppraisalAnalyst { get; set; } = true;
        public bool AppraisalLAP { get; set; } = true;
        public bool Televerification { get; set; } = true;


    }

    public class AppraisalModal : BaseModel
    {

        public long ClientApproachId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid LoanType.")]
        [Required(ErrorMessage = "Please select LoanType.")]
        public long LoanTypeId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid AppraisalType.")]
        [Required(ErrorMessage = "Please select AppraisalType.")]
        public string AppraisalTypeId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter Zone.")]
        [Required(ErrorMessage = "Please type Zone.")]
        public long ZoneId { get; set; }

        public string ZoneName { get; set; }
        // [Required(ErrorMessage = "Please enter name as per PAN.")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters required.")]
        [StringLength(100, ErrorMessage = "Invalid name as per PAN.")]
        public string AliasName { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid Gender.")]
        [Required(ErrorMessage = "Please select Gender.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        //[RegularExpression(@"/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/" ,ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid MaritalStatus.")]
        [Required(ErrorMessage = "Please select MaritalStatus.")]

        public long MaritalStatusId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid Caste.")]
        [Required(ErrorMessage = "Please select Caste.")]

        public long CasteId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid Religion.")]
        [Required(ErrorMessage = "Please select Religion.")]

        public long ReligionId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid Literacy.")]
        [Required(ErrorMessage = "Please select Literacy.")]

        public long LiteracyId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid TotalFamilyMembers.")]
        [Required(ErrorMessage = "Please enter TotalFamilyMembers.")]
        public byte TotalFamilyMembers { get; set; }
        //[Range(1, long.MaxValue, ErrorMessage = "Please enter valid NoOfDependants.")]
        //[Required(ErrorMessage = "Please enter NoOfDependants.")]
        //[RequiredIf("TotalFamilyMembers",1,">",ErrorMessage = "Please enter valid NoOfDependants.")]
        public byte NoOfDependants { get; set; }
        [Range(2, long.MaxValue, ErrorMessage = "Please enter valid ResidingYears.")]
        [Required(ErrorMessage = "Please enter ResidingYears.")]
        public int ResidingYears { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid ResidentialStatus.")]
        [Required(ErrorMessage = "Please select ResidentialStatu.")]

        public long ResidentialStatusId { get; set; }
        public string AppraisalDate { get; set; }

        [Required(ErrorMessage = "Please enter Name.")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters required.")]
        [StringLength(50, ErrorMessage = "Invalid name length.")]
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Minimum 3 characters required.")]
        [StringLength(50, ErrorMessage = "Invalid name length.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [RegularExpression(RegExpr.MobileNo, ErrorMessage = "Invalid mobile number.")]
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Code { get; set; }
        public string AppraisalCode { get; set; }
        public long StaffId { get; set; }
        public int Age { get; set; }
        public string ClientName { get; set; }
        public long PreviousAppraisalId { get; set; }
        [Required]
        //[RegularExpression(@"^(\d{1,1}\.\d{1,3})$", ErrorMessage ="Invalid format.It must be like [#.#] ex: 5.0/5.65/5.658")]
        [Range(3, 7, ErrorMessage = "Please enter valid height.")]
        public decimal HeightInFeet { get; set; }
        [Required]
        // [RegularExpression(@"^(\d{1,3}\.\d{1,2})$", ErrorMessage = "Invalid format.It must be like [##.##] ex: 55.00/65.50/75.65")]
        [Range(1, 200, ErrorMessage = "Please enter valid weight.")]
        public decimal WeightInKg { get; set; }
        //[Required(ErrorMessage = "Enter valid spouce name")]
        [RequiredIf("MaritalStatusId", 2, "!=")]
        [StringLength(100)]
        [RegularExpression(RegExpr.PersonName, ErrorMessage = "Enter valid spouce name")]
        public string SpouceName { get; set; }
        [Required(ErrorMessage = "Enter valid father name")]
        [StringLength(100)]
        [RegularExpression(RegExpr.PersonName, ErrorMessage = "Enter valid father name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Enter valid mother name")]
        [StringLength(100)]
        [RegularExpression(RegExpr.PersonName, ErrorMessage = "Enter valid mother name")]
        public string MotherName { get; set; }
        public string OTP { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageExtension { get; set; }
        public ApplicationTabDisplayModel TabDisplayModel { get; set; }
        public List<AppraisalProfileKYCDetail> AppraisalKYCDetails { get; set; }
        public IEnumerable<IdNameModel> KYCTypes { get; set; }
        public bool IsMobileNoVerified { get; set; } = false;
        public string Branch { get; set; }
        public string State { get; set; }
        public decimal RequestedAmount { get; set; }



        public string EncryptId { get; set; }
        public string EncryptScreenType { get; set; }


        public IEnumerable<IdNameModel> ClientApproches { get; set; }

        public AppraisalMastersModel AppraisalMastersModel { get; set; }
        public AppraisalModal()
        {
            AppraisalKYCDetails = new List<AppraisalProfileKYCDetail>();
            AppraisalMastersModel = new AppraisalMastersModel();
            KYCTypes = new List<IdNameModel>();
        }
       

    }
    public class AppraisalMastersModel
    {
        public IEnumerable<IdNameModel> Literacies { get; set; }
        public IEnumerable<IdNameModel> LoanTypes { get; set; }
        public IEnumerable<IdNameModel> Religions { get; set; }
        public IEnumerable<IdNameModel> EnquiryTypes { get; set; }
        public IEnumerable<IdNameModel> Districts { get; set; }
        public IEnumerable<IdNameModel> ResidentialStatus { get; set; }
        public IEnumerable<IdNameModel> Zones { get; set; }
        public IEnumerable<IdNameModel> MaritalStatus { get; set; }
        public IEnumerable<IdNameModel> Caste { get; set; }
    }
    public class AppraisalProfileKYCDetail
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Invalid KYC Type.")]
        public long KYCTypeId { get; set; }
        [Required(ErrorMessage = "Invalid KYC document number.")]
        public string KYCDocumentNo { get; set; }
        public bool IsAddressProof { get; set; }
        public string KYCType { get; set; }
    }

    public class LoanDetails
    {
        public long ProductId { get; set; }
        public string LoanAccountNumber { get; set; }
        public DateTime LoanDate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal Interest { get; set; }
        public string Frequency { get; set; }
        public IEnumerable<IdNameModel> Products { get; set; }
    }
}
