namespace AdithyaBank.BackEnd.Entities;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static AdithyaBank.BackEnd.Models.Enums;
public class User : IdentityUser
{
    public string RoleId { get; set; }
    public  Role Role { get; set; }


}