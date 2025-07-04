using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class Districts:BaseEntity
    {
        public string Name { get; set; }
        public long StateId { get; set; }
        public States States { get; set; }
        public ICollection<ApplicationRegister> ApplicationRegister { get; set; }
    }
}
