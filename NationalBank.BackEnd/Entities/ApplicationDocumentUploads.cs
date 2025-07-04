using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Entities
{
    public class ApplicationDocumentUploads:BaseEntity
    {
        public long? ApplicationId { get; set; }
        public string DocumentName { get; set; }
        public long DocumentTypeId { get; set; }
        public ApplicationRegister ApplicationRegister { get; set; }
        public DocumentTypes DocumentTypes { get; set; }
    }
}
