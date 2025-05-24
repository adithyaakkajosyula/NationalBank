using AdithyaBank.BackEnd.Extensions;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace AdithyaBank.FrontEnd.Controllers
{
    public class ApplicationController : BaseController
    {
        private ICommonRepository _commonRepository;
        private IApplicationRegisterRepository _applicationRegisterRepository;
        public ApplicationController(IApplicationRegisterRepository applicationRegisterRepository, ICommonRepository commonRepository)
        {
            _applicationRegisterRepository = applicationRegisterRepository;
            _commonRepository = commonRepository;   
        }
        public async Task<IActionResult> AddApplication(string id = "0", string screenType = "0", bool viewOnly = false)
        {
            var tabDisplayModel = _commonRepository.SetAppraisalTabDisplay(_commonRepository.Decrypt<int>(screenType));
            TempData.Put<ApplicationTabDisplayModel>("display", tabDisplayModel);
            TempData["AdminForDisplayOnly"] = viewOnly;
            long appraisalId = _commonRepository.Decrypt<long>(id);

            AppraisalModal modal = new AppraisalModal();
            modal.KYCTypes =await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.LoanTypes = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.MaritalStatus = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.ResidentialStatus = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.Literacies = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.Caste = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.Districts = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.EnquiryTypes = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.Religions = await _commonRepository.GetDocumentTypes();
            modal.AppraisalMastersModel.Zones = await _commonRepository.GetDocumentTypes();
            modal.Id = appraisalId;
            modal.EncryptId = id;
            modal.EncryptScreenType = screenType;

            return View("AddApplication", modal);
        }
        public async Task<IActionResult> Getdata(string term)
        {
            var data = await _applicationRegisterRepository.Getdata(term);
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> Getappraisal(long id)
        {
            var result = await _applicationRegisterRepository.GetApplicantDetails(id);  
            return Json(result);        
        }
    }
}
