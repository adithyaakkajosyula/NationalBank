using NationalBank.Api.Filters;
using NationalBank.BackEnd.Authorization;
using NationalBank.BackEnd.Extensions;
using NationalBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.DataAnnotations;


namespace NationalBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDataServiceController : ControllerBase
    {
        private ICommonRepository _commonRepository;
        private readonly ILogger<ApiDataServiceController> _logger;
        public ApiDataServiceController(ICommonRepository commonRepository, ILogger<ApiDataServiceController> logger)
        {
            _commonRepository = commonRepository;
            _logger = logger;
        }
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _commonRepository.GetCountries();
            if (countries is null)
                return NotFound("Internal Error");

            // Return the raw list plus just two top‑level links
            var response = new
            {
                data = countries,   // rename to “items” or “countries” if you prefer
                links = new[]
                {
            new
            {
                rel    = "states",
                href   = Url.Action(nameof(GetStates),    // GET /states
                                    "ApiDataService"),
                method = "GET"
            },
            new
            {
                rel    = "districts",
                href   = Url.Action(nameof(GetDistricts), // GET /districts
                                    "ApiDataService"),
                method = "GET"
            }
        }
            };

            return Ok(response);
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

        [ProducesResponseType(404)]
        [HttpGet("getcountries")]
        public async Task<IActionResult> GetStatesbyCountry([FromHeader]long countryId)
        {
            var result = await _commonRepository.GetDistrictTypes(countryId);
            if (result == null) { throw new InvalidOperationException("Result was null."); }
            if (result.Count() == 0) { return NotFound("No Districts Found For CountryId"); }
            return Ok(result);
        }

        [ServiceFilter(typeof(APIIResourceFilter))]
        [ServiceFilter(typeof(APIIActionFilter))]
        [ServiceFilter(typeof(APIIResultFilter))]
        [HttpPost("SendDetails")]
        public async Task<IActionResult> SendDetails([FromBody] Student product, CancellationToken cancellationToken)
        {
            try
            {
                // Simulate a delay
                await Task.Delay(10000, cancellationToken); // Cancel if client drops

                return Ok(product);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("❌ Request was cancelled at {Time}", DateTime.Now);
                return BadRequest("Request was cancelled by client.");
            }
        }

        [HttpGet("secured")]
        [CustomAuthorize(Policy = "MinimumAge18")]
        public IActionResult GetSecuredData()
        {
            return Ok("This is protected by age-based policy");
        }

        [EnableQuery]                         // activates $filter/$orderby…
        [HttpGet]                             // matches GET /odata/Employees
        public IQueryable<Employee> Get() => GetEmployeesList().AsQueryable();
        private List<Employee> GetEmployeesList()
        {
            var employeeslist = new List<Employee>
                {
                       new Employee(){Id= 1,Name= "Adithya",Age = 55,Sex = 'M',Dob =new DateTime(2024,05,02),Salary=50000,DepartmentId = 1, Skills = new List<string> { "C#", "SQL" }  },
                       new Employee(){Id= 1,Name= "Pavan",Age = 44,Sex = 'M',Dob =new DateTime(2023,05,02) ,Salary = 20000 , DepartmentId = 1 , Skills = new List<string> { "Pyhton", "No sql" } },
                       new Employee(){Id= 1,Name= "Divya",Age = 22,Sex = 'F',Dob =new DateTime(2022,05,02),Salary = 30000 , DepartmentId = 1 ,  Skills = new List<string> { "C#", "SQL" } },
                       new Employee(){Id= 2,Name= "Lavanya",Age = 33,Sex = 'T',Dob =new DateTime(2021,05,02) ,Salary = 10000 , DepartmentId = 2 ,  Skills = new List<string> { "C", "Post-SQL" } },
                       new Employee(){Id= 2,Name= "Sridar",Age = 66,Sex = 'M',Dob =new DateTime(2020,05,02),Salary = 80000 , DepartmentId = 1 ,  Skills = new List<string> { "C++", "Oracle" } },
                       new Employee(){Id= 3,Name= "Hari",Age = 11,Sex = 'F',Dob =new DateTime(2019,05,02) , Salary = 70000 ,DepartmentId = 1 ,  Skills = new List < string > { "Javascript", "SQL" }},
                       new Employee(){Id= 3,Name= "Jagan",Age = 66,Sex = 'M',Dob =new DateTime(2018,05,02),Salary = 15000 , DepartmentId = 1 ,  Skills = new List<string> { "", "SQL" }  },
                       new Employee(){Id= 3,Name= "Dinesh",Age = 00,Sex = 'F',Dob =new DateTime(2017,05,02),Salary = 40000 ,DepartmentId = 2 ,  Skills = new List < string > { "C#", "SQL" }}
                };

                return employeeslist;
        }

    }

    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
        public DateTime Dob { get; set; }
        public decimal Salary { get; set; }
        public long DepartmentId { get; set; }
        public List<string> Skills { get; set; }
    }
    public class Department
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class Student
    {
        [Required(ErrorMessage = "Please give Id value")]
        public int Id { get; set; }
        [NoSpecialChars(ErrorMessage = "Comment cannot contain special characters.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name Must Be Minimum 5 Characters")]
        public string Name { get; set; }
        [Range(25, 75, ErrorMessage = "Age Must Be Between 25 and 75")]
        public int Age { get; set; }
        [EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[@#$%^&*!])(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).+$", ErrorMessage = "Password must be one capital letter one small letter one number one symbol")]
        public string Password { get; set; }
        public string ComparePassword { get; set; }
        public long Phone { get; set; }
    }

   

    
}
