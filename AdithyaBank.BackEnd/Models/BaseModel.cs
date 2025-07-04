using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBank.BackEnd.Extensions;
using Newtonsoft.Json;
namespace NationalBank.BackEnd.Models
{
    public class BaseModel
    {
        [JsonConverter(typeof(IdToStringConverter))]
        public long Id { get; set; }
    }
    public class AudioModel
    {
        public string songname { get; set; }
        public string base64 { get; set; }    
    }
    public class IdNameModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class IdNameModelForAuuthentication
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class StatesModel:BaseModel
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
    }
    public class DistrictsModel : BaseModel
    {
        public long StateId { get; set; }
        public string Name { get; set; }
    }

}
