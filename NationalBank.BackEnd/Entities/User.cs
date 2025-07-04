namespace NationalBank.BackEnd.Entities;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static NationalBank.BackEnd.Models.Enums;
public class User : IdentityUser
{
    public string RoleId { get; set; }
    public  Role Role { get; set; }
    public DateTime Dob { get; set; }
}