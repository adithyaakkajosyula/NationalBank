using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() =>
        Ok("v1 data – access granted (18+).");
    }
}
