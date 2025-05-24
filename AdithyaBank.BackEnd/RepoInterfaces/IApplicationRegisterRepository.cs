using AdithyaBank.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.RepoInterfaces
{
    public interface IApplicationRegisterRepository
    {
        Task<BaseResultModel> ApplicationRegisterAdd(ApplicationRegisterModel model);
        Task<IEnumerable<IdNameModel>> Getdata(string query);
        Task<GetApplicantDetailsModel> GetApplicantDetails(long applicantid);
        Task<List<ApplicationRegisterModel>> GetAppraisalsList();
        Task<FileDownloadResult> ViewOrDownload(long id, long documentid);
        Task<FileDownloadWithByteArrayResult> ReadFileAsync(long id, long documentid);
        Task<BaseResultModel> Saveappraisallist(List<ApplicationRegisterModel> models);
        Task<BaseResultModel> Deletefromappraisallist(long id);
    }
}
