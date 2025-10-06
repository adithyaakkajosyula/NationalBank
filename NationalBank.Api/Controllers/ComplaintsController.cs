using Microsoft.AspNetCore.Mvc;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;

namespace NationalBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        public readonly IComplaintsRepository _complaintsRepository;

        public ComplaintsController(IComplaintsRepository complaintsRepository)
        {
            _complaintsRepository = complaintsRepository;
        }
        [HttpGet("GetComplaints")]
        public async Task<IActionResult> GetComplaints(int pageNumber = 1, int pageSize = 10)
        {
            await Task.Delay(3000);
            var complaints = await _complaintsRepository.GetComplaints(pageNumber, pageSize);
            return Ok(complaints);
        }

    }
}
