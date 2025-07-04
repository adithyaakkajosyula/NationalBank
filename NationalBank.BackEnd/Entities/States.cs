using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class States:BaseEntity
    {
        public string Name { get; set; }
        public long CountryId { get; set; }
        public Countries Countries { get; set; }
        public ICollection<Districts> Districts { get; set; }
        public ICollection<ApplicationRegister> ApplicationRegister { get; set; }
    }
}
