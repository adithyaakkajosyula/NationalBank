using Microsoft.AspNetCore.Http;
using NationalBank.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.RepoInterfaces
{
    public interface ICommonRepository
    {
        ApplicationTabDisplayModel SetAppraisalTabDisplay(int screenFor);
        string Encrypt(string plainText);
        T Decrypt<T>(string enctyptedText);
        Task<IList<IdNameModel>> GetDocumentTypes();
        Task<IList<IdNameModel>> GetCountryTypes(string query);
        Task<IList<IdNameModel>> GetStateTypes(long countryid);
        Task<IList<IdNameModel>> GetDistrictTypes(long stateid);
        Task<IList<IdNameModel>> GetProducts();
        Task<IList<IdNameModel>> GetCountries();
        Task<IList<StatesModel>> GetStates();
        Task<IList<DistrictsModel>> GetDistricts();
        Task<BaseResultModel> UploadFile(IFormFile file, string path, CancellationToken ct = default);
    }
}
