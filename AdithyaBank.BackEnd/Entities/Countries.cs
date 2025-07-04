using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class Countries:BaseEntity
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public ICollection<States> States { get; set; }
        public ICollection<ApplicationRegister> ApplicationRegister { get; set; }
    }
}
