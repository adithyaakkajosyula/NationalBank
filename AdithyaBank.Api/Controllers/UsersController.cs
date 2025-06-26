namespace AdithyaBank.Api.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.Repositories;
using static AdithyaBank.BackEnd.Models.Enums;
using AdithyaBank.BackEnd.Entities;
using static AdithyaBank.BackEnd.Models.Constants;

[CustomAuthorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    public UsersController(IUserRepository userRepository, UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [CustomAllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        await Task.Delay(3000);
        var response = await _userRepository.Authenticate(model);
        return Ok(response);
    }

    [CustomAuthorize(new string[] { RolesList.Admin, RolesList.HR })]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userRepository.GetAll();
        return Ok(users);
    }
    //[Authorize(RolesList.Admin)]
    [CustomAuthorize]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            return NotFound("User Not Found");  
        }
        return Ok(user);
    }
    [CustomAllowAnonymous]
    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        try
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var roleExists = await _roleManager.FindByIdAsync(model.RoleId);
            if (roleExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Not Exists" });
            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                RoleId = model.RoleId,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        catch(Exception ex)
        {
            throw ex;
        }
        
    }

}