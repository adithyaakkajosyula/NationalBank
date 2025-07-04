using NationalBank.BackEnd.Extensions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NationalBank.BackEnd.Repositories
{
    public class MusicPlayerRepository
    {
        public class AudiofilesModel
        {
            public List<string> Files { get; set; }
        }

        public List<System.IO.FileInfo> AudioController()
        {
            List<System.IO.FileInfo> music =  new DirectoryInfo("D:\\").FilesViaPattern("mp3");
            return music;   
        }
    }
}
