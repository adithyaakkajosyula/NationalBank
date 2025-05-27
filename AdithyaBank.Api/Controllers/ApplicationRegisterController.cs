using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using static AdithyaBank.BackEnd.Models.Enums;

namespace AdithyaBank.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRegisterController : ControllerBase
    {
        private readonly IApplicationRegisterRepository _applicationRegisterRepository; 

        public ApplicationRegisterController(IApplicationRegisterRepository applicationRegisterRepository)
        {
            _applicationRegisterRepository = applicationRegisterRepository; 
        }

        [HttpPost("AddApplication")]
        public async Task<BaseResultModel> AddApplication([FromForm] ApplicationRegisterModel model)
        {
            var result = await _applicationRegisterRepository.ApplicationRegisterAdd(model);
            return result;
        }
        [HttpGet("getapplications/{applicantid}")]
        public async Task<IActionResult> GetApplicantDetails(long applicantid)
        {
            // only admins can access other user records
            var currentUser = (UserModel?)HttpContext.Items["User"];
            if (currentUser?.Role.Id == (RoleRequired.Admin.GetDisplayName()).ToString())
                return Unauthorized(new { message = "Unauthorized" });
            var applicantdetails = await _applicationRegisterRepository.GetApplicantDetails(applicantid);

            return Ok(applicantdetails);
        }

        [HttpGet("GetApplicationsList")]
        public async Task<IActionResult> GetApplicationsList()
        {
            var applicationslist = await _applicationRegisterRepository.GetAppraisalsList();
            return Ok(applicationslist);
        }
    }
}
