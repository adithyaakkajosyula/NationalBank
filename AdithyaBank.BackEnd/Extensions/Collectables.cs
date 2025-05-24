using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Extensions
{
    public static class Collectables
    {
        public static List<System.IO.FileInfo> FilesViaPattern(this System.IO.DirectoryInfo fldr, string pattern)
        {
            var filter = pattern.Split(" ");
            return fldr.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
                .Where(l => filter.Any(k => l.Name.EndsWith(k))).ToList();
        }
    }
}
