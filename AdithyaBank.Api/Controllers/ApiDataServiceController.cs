using AdithyaBank.Api.Filters;
using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.DataContext;
using AdithyaBank.BackEnd.Entities;
using AdithyaBank.BackEnd.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using static AdithyaBank.BackEnd.Models.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdithyaBank.Api.Controllers
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

        [ProducesResponseType(404)]
        [HttpGet("getcountries")]
        public async Task<IActionResult> GetStatesbyCountry([FromHeader]long countryId)
        {
            var result = await _commonRepository.GetDistrictTypes(countryId);
            if (result == null) { throw new InvalidOperationException("Result was null."); }
            if (result.Count() == 0) { return NotFound("No Districts Found For CountryId"); }
            return Ok(result);
        }

        [ServiceFilter(typeof(APIIActionFilter))]
        [HttpPost("SendDetails")]
        public async Task<IActionResult> SendDetails([FromBody] Student product, CancellationToken cancellationToken)
        {
            try
            {
                // Simulate a delay
                await Task.Delay(10000, cancellationToken); // Cancel if client drops

                return Ok("Data processed");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("❌ Request was cancelled at {Time}", DateTime.Now);
                return BadRequest("Request was cancelled by client.");
            }
        }




       // [ServiceFilter(typeof(CustomExceptionFilter))]
        [HttpGet("GetEmployeeDetailsByQueryParameter")]
        public async Task<ActionResult<Employee>> GetEmployeeDetailsByQueryParameter([FromQuery]int id)
        {
            try
            {
                // var emp = GetEmployeesList().Where(a => a.Id == id).First();
                int zero = 0;
                var emp = 1 / zero;

                if (emp == null) return NotFound();

                return Ok(emp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
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

    public class NoSpecialCharsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str)) return true;
            return !str.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }

    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Customize the response sent when an unhandled exception occurs
            context.Result = new ObjectResult(new { message = "An error occurred.", details = context.Exception.Message })
            {
                StatusCode = 500
            };

            // Prevent the exception from propagating further
            context.ExceptionHandled = true;
        }
    }
}
