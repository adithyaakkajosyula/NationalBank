using NationalBank.BackEnd.Extensions;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using NationalBank.BackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace NationalBank.UnitTest
{
    [TestClass]
    public class UnitTest1:BaseTest
    {
        private const double V = 50000.87;
        private IApplicationRegisterRepository _repository;    


        [TestInitialize]
        public override void InitTest()
        {
            base.InitTest();
            _repository = new ApplicationRegisterRepository(_context,_appSettings,_commonRepository);
          
        }
        [TestMethod]
        public async Task TestMethod1()
        {
            List<string> allFiles = new List<string>();
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
               
                if (drive != "C:\\")
                {
                    List<string> directfiles = Directory.EnumerateFiles(drive, "*.mp3*",
                   new EnumerationOptions
                   {
                       IgnoreInaccessible = true,
                       RecurseSubdirectories = true,
                   }).ToList();
                    allFiles.AddRange(directfiles);
                }

                
            }
            


            //  List<System.IO.FileInfo> music = new DirectoryInfo("D:\\adithya\\").FilesViaPattern("mp3");



            string filePath = Path.Combine(@"D:\adithya\New2\School\School.FrontEnd\wwwroot\images\p.jpg");
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IFormFile formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
                ContentDisposition = "name=\"File\"; filename=\"p.jpg\""
            };


            ApplicationRegisterModel model = new ApplicationRegisterModel()
            {
                Id = 0,
                ApplicationName = "adithya",
                ApplicationFatherName = "fathername",
                ApplicationMotherName = "mothername",
                ApplicationDob = new DateTime(2000, 4, 6),
                ApplicationGender = 'M',
                ApplicationQualification = "SSC",
                ApplicationMartialStatus = "Single",
                ApplicationMobile = "7673906058",
                ApplicationEmail = "adithyamech001@gmail.com",
                ApplicationRequestedAmount = 50000.90m,
                ApplicationHobbies = ("Playing,Reading,Walking").Split(',', StringSplitOptions.RemoveEmptyEntries),
                ApplicationRegisterDate = DateTime.Now, 
                ApplicationIsAcceptedTermsAndConditions = true,
                ApplicationDistrictId = 22,
                ApplicationStateId = 1,
                ApplicationCountryId = 150,   
                ApplicationAddress = "Address",
                DocumentFile = formFile

            };
            BaseResultModel result = await _repository.ApplicationRegisterAdd(model);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsSuccess);
        }
    }
}