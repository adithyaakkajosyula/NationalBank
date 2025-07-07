using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NationalBank.BackEnd.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;
using static NationalBank.BackEnd.Models.Constants;

namespace NationalBank.BackEnd.Repositories
{
    public class CommonRepository:ICommonRepository
    {
        private readonly IDataProtector _protector;
        private readonly NationalBankDatabaseContext _context;
        private const long MaxSize = 50L * 1024 * 1024;
        public CommonRepository(NationalBankDatabaseContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _protector = provider.CreateProtector(Constants.DataProtectionKey);
        }
        public ApplicationTabDisplayModel SetAppraisalTabDisplay(int screenFor)
        {
            AppraisalListFor appraisalTabFor = (AppraisalListFor)screenFor;
            ApplicationTabDisplayModel tabDisplayModel = new ApplicationTabDisplayModel();

            if (appraisalTabFor == AppraisalListFor.AppraisalEntry)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.AppraisalList)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.AppraisalApproval)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.BussinessVerification)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.ForwardToCB)
            {
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;
                //tabDisplayModel.Completion = false;


            }
            else if (appraisalTabFor == AppraisalListFor.CBEnquiry)
            {

            }
            else if (appraisalTabFor == AppraisalListFor.CBEnquiryUpload)
            {

            }
            else if (appraisalTabFor == AppraisalListFor.AppraisalAnalyst)
            {
                tabDisplayModel.Televerification = false;
                tabDisplayModel.ForwordToCB = false;
            }
            else if (appraisalTabFor == AppraisalListFor.AppraisalAdmin)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.Televerification = false;
                //tabDisplayModel.Completion = false;

            }
            else if (appraisalTabFor == AppraisalListFor.Score)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.Compare)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.Legal)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.CreditComments)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.CreditCommitee)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;


            }
            else if (appraisalTabFor == AppraisalListFor.ProvisionalAcceptance)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;


            }
            else if (appraisalTabFor == AppraisalListFor.FinalApproval)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
                tabDisplayModel.Televerification = false;

            }
            else if (appraisalTabFor == AppraisalListFor.Televerification)
            {
                tabDisplayModel.ForwordToCB = false;
                tabDisplayModel.AppraisalAnalyst = false;
            }
            return tabDisplayModel;

        }

        public string Encrypt(string plainText) => _protector.Protect(plainText);
        public T Decrypt<T>(string enctyptedText)
        {
            object decrypted = _protector.Unprotect(enctyptedText);
            return (T)Convert.ChangeType(decrypted, typeof(T));
        }
        public async Task<IList<IdNameModel>> GetDocumentTypes()
        {
            var documentTypes =await _context.DocumentTypes.Select(a => new IdNameModel() { Id  = a.Id,Name = a.Name}).ToListAsync();
            return documentTypes;
        }
        public async Task<IList<IdNameModel>> GetCountries()
        {
            var result = _context.Countries.Where(a => a.Rowstate < 3);
            return await result.Select(a => new IdNameModel()
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();
        }
        public async Task<IList<IdNameModel>> GetCountryTypes(string query)
        {
            var result = _context.Countries.Where(a => a.Name.Contains(query));
            return await result.Select(a => new IdNameModel()
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();
        }
        public async Task<IList<StatesModel>> GetStates()
        {
            var states = await _context.States.Where(a => a.Rowstate <3).Select(c => new StatesModel() { Id = c.Id, Name = c.Name,CountryId = c.CountryId }).ToListAsync();
            return states;
        }
        public async Task<IList<IdNameModel>> GetStateTypes(long countryid)
        {
            var states = await _context.States.Where(a=>a.CountryId == countryid).Select(c => new IdNameModel() { Id = c.Id, Name = c.Name }).ToListAsync();
            return states;
        }
        public async Task<IList<DistrictsModel>> GetDistricts()
        {
            var districts = await _context.Districts.Where(a => a.Rowstate < 3).Select(c => new DistrictsModel() { Id = c.Id, Name = c.Name,StateId = c.StateId }).ToListAsync();
            return districts;
        }
        public async Task<IList<IdNameModel>> GetDistrictTypes(long stateid)
        {
            var districts = await _context.Districts.Where(a=>a.StateId == stateid).Select(c => new IdNameModel() { Id = c.Id, Name = c.Name }).ToListAsync();
            return districts;
        }
        public async Task<IList<IdNameModel>> GetProducts()
        {
            var products = await _context.Product.Where(a => a.Rowstate < 3).Select(a => new IdNameModel() { 
                Id = a.Id,
                Name = a.Type
            }).ToListAsync();
                 
            return products;
        }

        public async Task<BaseResultModel> UploadFile(IFormFile file,string path, CancellationToken ct = default)
        {
                if (file is null || file.Length == 0)
                    new BaseResultModel() { Id = 0, IsSuccess = false, Message = "File is empty or null." };

                if (file.Length > (long)MaxSizeLimit.MaxFileSize)
                    new BaseResultModel() { Id = 0, IsSuccess = false, Message = $"Max {MaxSize / 1024 / 1024} MB allowed." };

                using (FileStream fs = File.Open(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs,ct);
                }

                return new BaseResultModel()
                {
                    Id = 0,
                    IsSuccess = true,
                    Message = "File uploaded successfully.",
                };
        }
    }
}
