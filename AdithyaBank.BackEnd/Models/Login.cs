using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Models
{
    public class Login
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class JWTTokenResponse
    {
        public string Token
        {
            get;
            set;
        }
    }
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required]
        public string RoleId { get; set; }
        public IEnumerable<IdNameModelForAuuthentication> Roles { get; set; }
    }

}
