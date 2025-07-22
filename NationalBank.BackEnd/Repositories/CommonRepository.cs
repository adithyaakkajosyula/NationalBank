using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static NationalBank.BackEnd.Models.Enums;
using Microsoft.AspNetCore.Http;
using static NationalBank.BackEnd.Models.Constants;
using System.Net.Http;
using STJ = System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;


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
        public async Task<FileDownloadResult> DownloadFile(string path)
        {


            if (!File.Exists(path))
            {
                return new FileDownloadResult() { IsSuccess = false, Message = "File not found." };
            }
            //var contentType = GetContentType(path);
            var provider = new FileExtensionContentTypeProvider();
             var res =   provider.TryGetContentType(path, out string contentType);

            var memorystream = new MemoryStream();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                await fs.CopyToAsync(memorystream);
                memorystream.Position = 0;
                return new FileDownloadResult()
                {
                    FileStream = memorystream,
                    FileContent = contentType,
                    FileName = $"{Path.GetFileName(path) + "." + Path.GetExtension(path).ToLowerInvariant()}",
                    IsSuccess = true,
                    Message = ""
                };
            }
        }
        public string GetContentType(string filePath)
        {
            // Logic to determine content type based on file content or extension
            // For example, you can use libraries like MimeMapping.GetMimeMapping(filePath) to determine content type
            // Or use a simple logic based on file extension
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                default:
                    return "application/octet-stream"; // Default content type for unknown files
            }
        }

        private readonly string _karzaApiKey = ""; // Set your API key here

        private static readonly STJ.JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

/*        public async Task<TResult?> GetAsync<TResult>(string uri)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-karza-key", _karzaApiKey);

            using var response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(((int)response.StatusCode).ToString());
            }

            var json = await response.Content.ReadAsStringAsync();
            return STJ.JsonSerializer.Deserialize<TResult>(json, _jsonOptions);
        }

        public async Task<TResult> PostAsync<TValue, TResult>(string uri, TValue value, string dateFormatString = "dd-MM-yyyy")
        {
            string requestJson = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = dateFormatString

            });
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                //client.Timeout= TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Add("x-karza-key", _karzaApiKey);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await client.PostAsync(uri, requestContent);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception(Convert.ToInt32(httpResponseMessage.StatusCode).ToString());
                }

                HttpContent content = httpResponseMessage.Content;
                //string str =await content.ReadAsStringAsync(); 
                return await content.ReadAsAsync<TResult>();
            }

        }*/
    }
}
