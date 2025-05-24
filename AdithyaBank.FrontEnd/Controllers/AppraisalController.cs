using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdithyaBank.FrontEnd.Controllers
{
    


    public class AppraisalController : BaseController
    {
        private readonly IApplicationRegisterRepository _applicationRegisterRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IOptions<AppSettings> _appSettings;
        public AppraisalController(IApplicationRegisterRepository applicationRegisterRepository, ICommonRepository commonRepository,IOptions<AppSettings> appSettings)
        {
            _applicationRegisterRepository = applicationRegisterRepository;
            _commonRepository = commonRepository;
            _appSettings = appSettings; 
        }
        [HttpGet]
        public async Task<IActionResult> AddAppraisal()
        {
            var model = new ApplicationRegisterModel()
            {
                ApplicationRegisterDate = DateTime.Now,
                ApplicantDocumentTypes  = await _commonRepository.GetDocumentTypes(),
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddAppraisalByclientside()
        {
            var model = new ApplicationRegisterModel()
            {
                ApplicationRegisterDate = DateTime.Now,
                ApplicantDocumentTypes = await _commonRepository.GetDocumentTypes(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAppraisal(ApplicationRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _applicationRegisterRepository.ApplicationRegisterAdd(model);
                if (result.IsSuccess == true)
                {
                    if (Request.Headers["Accept"].ToString().Contains("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(result);
                    }
                    else
                    {
                        // If not JSON, return a view
                        return WithSuccess(result.Message).RedirectToAction("AddAppraisal");
                    }
                   
                }
                else
                {
                    TempData.AddAlertWarning(result.Message);
                    model.ApplicantDocumentTypes = await _commonRepository.GetDocumentTypes();
                    return View(model);
                }
            }
            else
            {
                TempData.AddAlert(new Alert() {AlertClass = "alert-danger",Message="validation error" });
                model.ApplicantDocumentTypes = await _commonRepository.GetDocumentTypes();
                return View(model);
            }
               
        }
        public async Task<IActionResult> GetCountries(string term)
        {
            var result = await _commonRepository.GetCountryTypes(term);
            return Json(result);
        }
        public async Task<IActionResult> Getstates(long countryId)
        {
            var result = await _commonRepository.GetStateTypes(countryId);
            return Json(result);
        }
        public async Task<IActionResult> GetDistricts(long stateId)
        {
            var result = await _commonRepository.GetDistrictTypes(stateId);
            return Json(result);
        }

        public async Task<IActionResult> AppraisalList()
        {
            var result = await _applicationRegisterRepository.GetAppraisalsList();
            return View(result);

        }
        public async Task<IActionResult> ViewFile(long id, long documentId)
        {
            var result = await _applicationRegisterRepository.ViewOrDownload(id,documentId);
            if (result.IsSuccess == false)
            {
                return View("_NoDocument",result.Message);
            }
            return File(result.FileStream,result.FileContent);          
        }
        public async Task<IActionResult> DownLoadFile(long id, long documentId)
        {
            var result = await _applicationRegisterRepository.ViewOrDownload(id, documentId);
            var contentType = "application/octet-stream";
            if (result.IsSuccess == false)
            {
                return View("_NoDocument", result.Message);
            }
            return File(result.FileStream, contentType,result.FileName);
        }

        public async Task<IActionResult> ViewFileByClientSide(long id,long documentId)
        {
            var result = await _applicationRegisterRepository.ReadFileAsync(id,documentId);

            return Json(result);    
        }
        [HttpPost]
        public async Task<IActionResult> Saveallappraisals(List<ApplicationRegisterModel> model)
        {
            var result = await _applicationRegisterRepository.Saveappraisallist(model);

            return Json(result);
        }

        public async Task<IActionResult> DeleteFromappraisalList(long id)
        {
            var result = await _applicationRegisterRepository.Deletefromappraisallist(id);
            return Json(result);
        }
    }
}
