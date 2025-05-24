using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.FrontEnd.Controllers
{
    public class TestController : BaseController
    {
        private IApplicationRegisterRepository _applicationRegisterRepository;
        public TestController(IApplicationRegisterRepository applicationRegisterRepository)
        {
            _applicationRegisterRepository = applicationRegisterRepository;
        }
        public IActionResult TestIndex()
        {
            return View();
        }
        public async Task<IActionResult> Getdata(string term)
        {
            var data = await _applicationRegisterRepository.Getdata(term);
            return Json(data);
        }
    }
}
