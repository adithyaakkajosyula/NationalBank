
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Entities
{
    public class DocumentTypes:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ApplicationDocumentUploads> ApplicationDocumentUploads { get; set; }
    }
}
