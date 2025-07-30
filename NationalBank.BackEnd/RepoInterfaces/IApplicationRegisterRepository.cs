using NationalBank.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.RepoInterfaces
{
    public interface IApplicationRegisterRepository
    {
        Task<BaseResultModel> ApplicationRegisterAdd(ApplicationRegisterModel model);
        Task<IEnumerable<IdNameModel>> Getdata(string query);
        Task<GetApplicantDetailsModel> GetApplicantDetails(long applicantid);
        Task<List<ApplicationRegisterModel>> GetAppraisalsList();
        Task<FileDownloadResult> ViewOrDownload(long id);
        Task<FileDownloadWithByteArrayResult> ReadFileAsync(long id, long documentid);
        Task<BaseResultModel> Saveappraisallist(List<ApplicationRegisterModel> models);
        Task<BaseResultModel> Deletefromappraisallist(long id);
    }
}
