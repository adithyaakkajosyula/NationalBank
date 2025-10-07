using Microsoft.AspNetCore.Mvc;
using NationalBank.BackEnd.RepoInterfaces;

namespace NationalBank.FrontEnd.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly IComplaintsRepository _complaintsRepository;   
        public ComplaintsController(IComplaintsRepository complaintsRepository)
        {
            _complaintsRepository = complaintsRepository;
        }
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 20)
        {
            await Task.Delay(3000);
            var list = await _complaintsRepository.GetComplaintsByStoredProcedure(pageNumber,pageSize);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Return only the partial HTML (for append)
                return PartialView("_ComplaintsPartial", list);
            }
            // Initial full page load
            return View(list);
        }
    }
}
