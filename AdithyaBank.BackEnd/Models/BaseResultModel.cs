using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Models
{
    public class BaseResultModel:BaseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }
    public class ApiBaseResultModel
    {
        public string Id { get; set; }
        public object Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }

    }
    public class FileDownloadResult : BaseResultModel
    {
        public MemoryStream FileStream { get; set; }
        public string FileContent { get; set; }
        public string FileName { get; set; }

    }

    public class FileDownloadWithByteArrayResult : BaseResultModel
    {
        public byte[] FileBytes { get; set; }
        public string FileContent { get; set; }
        public string FileName { get; set; }
    }
}
