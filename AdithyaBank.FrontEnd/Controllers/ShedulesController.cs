using AdithyaBank.BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.FrontEnd.Controllers
{
    public class ShedulesController : Controller
    {
        public IActionResult ShedulesAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ShedulesAdd( ActionContext e)
        {
            BaseResultModel result = new BaseResultModel();
            return View(result);
        }
    }
}
