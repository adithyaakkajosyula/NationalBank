using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.Models
{
    public class AppSettings
    {
        public string IdentityAuthority { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string ApplicationDataDbConnectionString { get; set; }

        public string CMSDbConnectionString { get; set; }

        public string Organization { get; set; }

        public string ImagesPath { get; set; }

        public int ConnectionTimeout { get; set; }

        public JWT JWT { get; set; }
        public string AzureBlobConnectionString { get; set; }
        public string AzureBlobContainer { get; set; }
        public string AzureLogConnectionString { get; set; }
        public string AzureLogContainer { get; set; }
    }
    public class JWT
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
