
namespace AdithyaBank.BackEnd.Repositories;

using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.DataContext;
using AdithyaBank.BackEnd.Entities;
using AdithyaBank.BackEnd.Helpers;
using AdithyaBank.BackEnd.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public interface IUserRepository
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    IEnumerable<GetUsers> GetAll();
    Task<UserModel> GetById(string id);
    Task<List<IdNameModelForAuuthentication>> GetRoles();
}

public class UserRepository : IUserRepository
{
    private readonly AdithyaBankIdentityDbContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    public UserRepository(
        AdithyaBankIdentityDbContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }


    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _context.User.Include(a=> a.Role).SingleOrDefaultAsync(x => x.UserName == model.Username);

        // validate
        if (user == null)
            return new AuthenticateResponse() {IsSuccess = false,Message="User Not Exists" };

        var passwordHasher = new PasswordHasher<User>();

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if(result == PasswordVerificationResult.Failed) return new AuthenticateResponse() { IsSuccess = false, Message = "Incorrect Password" };

        // authentication successful so generate jwt token
        var usermodel = new UserModel()
        {
            Id = user.Id,
            Username = user.UserName,
            Role = new RoleModel()
            {
                Id = user.Role.Id,
                Name = user.Role.Name
            },
            IsSuccess = true,
            Message = "User Data Fetched Sucessfully"
            
        };
        var jwtToken = _jwtUtils.GenerateJwtToken(usermodel);

        return new AuthenticateResponse() { Id = user.Id,Username = user.UserName,Role= new RoleModel() { Id = user.Role.Id,Name = user.Role.Name},Token = jwtToken,IsSuccess = true,Message="Login suscessfull" };
    }

    public IEnumerable<GetUsers> GetAll()
    {
        return _context.User.Select(a => new GetUsers() { 
                UserId = a.Id,
                RoleId = a.RoleId,
                UserName = a.UserName,
                RoleName = a.Role.Name
        });;
    }

    public async Task<UserModel> GetById(string id) 
    {
        var user = await _context.User.Include(a => a.Role).Where(b => b.Id == id).SingleOrDefaultAsync();

        if (user == null)
        {
            return new UserModel() { IsSuccess = false, Message = "User Not Found" };
        }
        else
        {
            return new UserModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Role = new RoleModel()
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name
                },
                IsSuccess = true,
                Message = "User Details Fetched Sucessfully"
            };
        }
        
    }
    public async Task<List<IdNameModelForAuuthentication>> GetRoles()
    {
        var roles = await _context.Role.Select(a => new IdNameModelForAuuthentication() { 
        
        Id = a.Id,
        Name = a.Name
        }).ToListAsync();
        return roles;   
    }
}