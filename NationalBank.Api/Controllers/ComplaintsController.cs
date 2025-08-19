using Microsoft.AspNetCore.Mvc;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;

namespace NationalBank.Api.Controllers
{
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        public readonly IComplaintsRepository _complaintsRepository;

        public ComplaintsController(IComplaintsRepository complaintsRepository)
        {
            _complaintsRepository = complaintsRepository;
        }
        [HttpGet("complaints")]
        public async Task<ActionResult<PagedResult<ComplaintsModel>>> GetComplaints(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _complaintsRepository.GetCommplaints(pageNumber, pageSize);  

            return Ok(result);
        }
    }
}
