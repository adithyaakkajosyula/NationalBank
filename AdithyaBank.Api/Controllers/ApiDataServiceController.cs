using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AdithyaBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDataServiceController : ControllerBase
    {
        private ICommonRepository _commonRepository;
        public ApiDataServiceController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _commonRepository.GetCountries();
            if (result ==null)
            {
                return NotFound("Internal Error");
            }

            return Ok(result);
        }
        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var result = await _commonRepository.GetStates();
            if (result == null)
            {
                return NotFound("Internal Error");
            }

            return Ok(result);
        }
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts()
        {
            var result = await _commonRepository.GetDistricts();
            if (result == null)
            {
                return NotFound("Internal Error");
            }

            return Ok(result);
        }

        [HttpGet("getcountries")]
        public async Task<IActionResult> GetStatesbyCountry([FromQuery]long countryId)
        {
            var result = await _commonRepository.GetDistrictTypes(countryId);
            if (result == null) { throw new InvalidOperationException("Result was null."); }
            if (result.Count() == 0) { return NotFound("No Districts Found For CountryId"); }
            return Ok(result);
        }


    }
}
