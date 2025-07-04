using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class Role :IdentityRole
    {
        public Role()
        {
            Users = new List<User>();
        }

        public Role(string roleName) : base(roleName) { }
        public List<User> Users { get; set; }
    }
}
