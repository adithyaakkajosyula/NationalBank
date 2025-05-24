using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.FrontEnd.Controllers
{
    [Authorize]
    public class GetApplicationsController : BaseController
    {
        private IApplicationRegisterRepository _applicationRegisterRepository;
        private ICommonRepository _commonRepository;
        public GetApplicationsController(IApplicationRegisterRepository applicationRegisterRepository,ICommonRepository commonRepository)
        {
            _applicationRegisterRepository = applicationRegisterRepository;
            _commonRepository = commonRepository;       
        }
        public async Task<IActionResult> GetApplicationDetails()
        {
            var model = new ApplicationGetModel()
            {
               LoanDetails = new LoanDetails() {Products = await _commonRepository.GetProducts() }
            };
            return View(model);
        }
        public async Task<IActionResult> Getdata(string term)
        {
            var data = await _applicationRegisterRepository.Getdata(term);
            return Json(data);
        }

        public async Task<IActionResult> Getapplicantdetails(long applicantid)
        {
            var data = await _applicationRegisterRepository.GetApplicantDetails(applicantid);
            return Json(data);
        }
    }
}
