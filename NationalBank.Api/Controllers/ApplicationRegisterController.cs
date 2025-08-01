using NationalBank.BackEnd.Authorization;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Extensions;
using static NationalBank.BackEnd.Models.Enums;

namespace NationalBank.Api.Controllers
{
    [CustomAuthorize]
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

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> DeleteAppraisal(long id)
        {
            var result = await _applicationRegisterRepository.Deletefromappraisallist(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("viewfile/{id}")]
        public async Task<IActionResult> ViewFile(long id)
        {
            var result = await _applicationRegisterRepository.ViewOrDownload(id);

            if (!result.IsSuccess || result.FileStream == null || string.IsNullOrEmpty(result.FileContent))
            {
                return NotFound(new { message = "Document not found." });
            }

            return File(result.FileStream, result.FileContent,result.FileName);
        }
    }
}
