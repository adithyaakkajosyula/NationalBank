using AdithyaBank.BackEnd.Entities;
using static AdithyaBank.BackEnd.Models.Enums;

namespace AdithyaBank.BackEnd.Models;

public class AuthenticateResponse :BaseResultModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public RoleModel Role { get; set; }
    public string Token { get; set; }
}