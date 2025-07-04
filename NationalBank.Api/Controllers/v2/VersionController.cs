using NationalBank.BackEnd.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace NationalBank.Api.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    // [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() =>
       Ok("v2 data – access granted (21+).");
    }
}
