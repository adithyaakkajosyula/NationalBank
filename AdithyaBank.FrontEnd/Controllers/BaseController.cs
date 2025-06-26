using AdithyaBank.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.FrontEnd.Controllers
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        protected internal BaseController WithSuccess(string Message)
        {
            TempData.AddAlert(new Alert() { Message = Message, AlertClass = "alert-success" });
            return this;
        }
        protected internal BaseController WithInfo(string Message)
        {
            TempData.AddAlert(new Alert() { Message = Message, AlertClass = "alert-info" });
            return this;
        }

        protected internal BaseController WithWarning(string Message)
        {
            TempData.AddAlert(new Alert() { Message = Message, AlertClass = "alert-warning" });
            return this;
        }

        protected internal BaseController WithError(string Message)
        {
            TempData.AddAlert(new Alert() { Message = Message, AlertClass = "alert-danger" });
            return this;
        }
    }
}
