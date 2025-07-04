namespace NationalBank.BackEnd.Models;

using NationalBank.BackEnd.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using static NationalBank.BackEnd.Models.Enums;

public class UserModel : ApiBaseResultModel
{
    public string RoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public DateTime Dob { get; set; }
    public RoleModel Role { get; set; }
}
public class RoleModel
{
    public string Id { get; set; }
    public string Name { get; set; }
}
public class GetUsers
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
}