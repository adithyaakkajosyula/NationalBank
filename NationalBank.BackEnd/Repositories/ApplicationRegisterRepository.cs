﻿using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Entities;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.RepoInterfaces;
using NationalBank.BackEnd.Repositories; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Storage.Blobs;
using static System.Net.Mime.MediaTypeNames;

namespace NationalBank.BackEnd.Repositories
{
    public class ApplicationRegisterRepository:IApplicationRegisterRepository
    {
        private readonly NationalBankDatabaseContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ICommonRepository _commonRepository;
        private readonly IWebHostEnvironment _env;
        public ApplicationRegisterRepository(NationalBankDatabaseContext context, IOptions<AppSettings> appSettings, ICommonRepository commonRepository, IWebHostEnvironment env)
        {
            _context = context;
            _appSettings = appSettings;
            _commonRepository = commonRepository;
            _env = env;
        }

        public async Task<BaseResultModel> ApplicationRegisterAdd(ApplicationRegisterModel model)
        {

            try
            {
                var applicationregister = await _context.ApplicationRegister.Include(a => a.ApplicationDocumentUploads).Where(b => b.Rowstate < 3 && b.Id == model.Id).FirstOrDefaultAsync();

                if (applicationregister == null)
                {
                    applicationregister = new ApplicationRegister();
                                        
                }
                if (applicationregister.ApplicationDocumentUploads == null)
                {
                    applicationregister.ApplicationDocumentUploads =  new ApplicationDocumentUploads();

                }
                applicationregister.Id = model.Id;  
                applicationregister.ApplicationName = model.ApplicationName;    
                applicationregister.Applicationfathername = model.ApplicationFatherName;
                applicationregister.Applicationmothername = model.ApplicationMotherName;
                applicationregister.Applicationdob = model.ApplicationDob;
                applicationregister.Applicationgender = model.ApplicationGender;
                applicationregister.Applicationqualification = model.ApplicationQualification;
                applicationregister.ApplicationMartialStatus = model.ApplicationMartialStatus;
                applicationregister.Applicationmobile = model.ApplicationMobile;
                applicationregister.Applicationemail = model.ApplicationEmail;
                applicationregister.ApplicationRequestedAmount = model.ApplicationRequestedAmount;
                applicationregister.ApplicationCountryId = model.ApplicationCountryId;
                applicationregister.ApplicationStateId = model.ApplicationStateId;
                applicationregister.ApplicationDistrictId = model.ApplicationDistrictId;
                applicationregister.ApplicationIsAcceptedTermsandConditions = model.ApplicationIsAcceptedTermsAndConditions;
                applicationregister.ApplicationAddress = model.ApplicationAddress;
                //applicationregister.ApplicationAddress = $"{_env.IsDevelopment()}-{(model.DocumentFile == null ? "false":"true")}-{_appSettings.Value.AzureBlobConnectionString}-{_appSettings.Value.AzureBlobContainer}";
                applicationregister.ApplicationHobbies = string.Join(",", model.ApplicationHobbies);
                applicationregister.ApplicationRegisterdate = model.ApplicationRegisterDate;
                applicationregister.ApplicationStatus = true;

                if (model.DocumentFile != null)
                {
                    applicationregister.ApplicationDocumentUploads.DocumentName = model.DocumentFile.FileName;
                    applicationregister.ApplicationDocumentUploads.DocumentTypeId = 2;                    
                }

                if (model.Id == 0)
                {
                    applicationregister.Rowstate = 1;
                    applicationregister.ApplicationDocumentUploads.Rowstate = 1;
                    await _context.ApplicationRegister.AddAsync(applicationregister);
                }
                else
                {
                    applicationregister.Rowstate = 2;
                    applicationregister.ApplicationDocumentUploads.Rowstate = 2;
                    _context.ApplicationRegister.Update(applicationregister);
                }
                await  _context.SaveChangesAsync();

                if (_env.IsDevelopment())
                {
                    string uploadsdocumentPath = Path.Combine(_appSettings.Value.ImagesPath, "NationalBanksDocuments", applicationregister.Id.ToString());

                    if (!Directory.Exists(uploadsdocumentPath))
                    {
                        Directory.CreateDirectory(uploadsdocumentPath);
                    }

                    if (model.DocumentFile != null)
                    {
                        string panfilePath = "";
                        if (model.DocumentFile.ContentType == "image/jpeg")
                        {
                            // This is for list of document upload
                            // panfilePath = Path.Combine(uploadsdocumentPath, $"{applicationregister.ApplicationDocumentUploads.ElementAt(0).ApplicationId}.jpg");
                            panfilePath = Path.Combine(uploadsdocumentPath, $"{applicationregister.ApplicationDocumentUploads.ApplicationId}.jpg");
                        }
                        else
                        {
                            panfilePath = Path.Combine(uploadsdocumentPath, $"{applicationregister.ApplicationDocumentUploads.ApplicationId}.pdf");
                        }


                        if (model.Id > 0)
                        {
                            // If File Exists in same path then nedd to delete file before overriding 
                            string jpgFilePath = Path.Combine(uploadsdocumentPath, $"{applicationregister.ApplicationDocumentUploads.ApplicationId}.jpg");
                            string pdfFilePath = Path.Combine(uploadsdocumentPath, $"{applicationregister.ApplicationDocumentUploads.ApplicationId}.pdf");
                            if (File.Exists(jpgFilePath))
                            {
                                File.Delete(jpgFilePath);
                            }
                            else if (File.Exists(pdfFilePath))
                            {
                                File.Delete(pdfFilePath);
                            }

                        }

                        using (var fileStream = new FileStream(panfilePath, FileMode.Create))
                        {
                            await model.DocumentFile.CopyToAsync(fileStream);
                        }

                    }
                }
                else
                {
                    if (model.DocumentFile != null)
                    {
                        // Save to Azure Blob
                        var blobServiceClient = new BlobServiceClient(_appSettings.Value.AzureBlobConnectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient(_appSettings.Value.AzureBlobContainer);

                        await containerClient.CreateIfNotExistsAsync();

                        // Determine file extension
                        string extension = model.DocumentFile.ContentType == "image/jpeg" ? ".jpg" : ".pdf";
                        string blobName = $"{applicationregister.ApplicationDocumentUploads.ApplicationId}{extension}";
                        string applicationIdPrefix = applicationregister.ApplicationDocumentUploads.ApplicationId.ToString();

                        // 🔍 Delete all existing blobs that start with the same ApplicationId
                        await foreach (var existingBlob in containerClient.GetBlobsAsync())
                        {
                            if (existingBlob.Name.StartsWith(applicationIdPrefix))
                            {
                                var existingBlobClient = containerClient.GetBlobClient(existingBlob.Name);
                                await existingBlobClient.DeleteIfExistsAsync();
                            }
                        }

                        // 💾 Upload the new blob
                        var blobClient = containerClient.GetBlobClient(blobName);

                        using (var stream = model.DocumentFile.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, overwrite: true);
                        }
                    }                                        
                }



                return new BaseResultModel() { IsSuccess = true, Message = $"Saved SucessFully with Id {applicationregister.Id}" };
            }
            catch ( Exception ex)
            {
                return new BaseResultModel() { IsSuccess=false,Message= ex.Message};
            }
           
        }

        public async Task<IEnumerable<IdNameModel>> Getdata(string query)
        {
            var result = _context.ApplicationRegister.Where(a => a.ApplicationName.Contains(query));
            return await result.Select(a => new IdNameModel()
            {
                Id = a.Id,
                Name = a.ApplicationName
            }).ToListAsync();
        }
        public async Task<GetApplicantDetailsModel> GetApplicantDetails(long applicantid)
        {
            var applicantdetails = await _context.ApplicationRegister.Include(a=> a.ApplicationDocumentUploads).Where(b=>b.Id == applicantid && b.Rowstate<3)
                .Select(a => new GetApplicantDetailsModel()
                {
                    Id = a.Id,
                    ApplicationName = a.ApplicationName,                    
                    ApplicationFatherName = a.Applicationfathername,
                    ApplicationMotherName = a.Applicationmothername,
                    ApplicationDob = a.Applicationdob,
                    ApplicationGender = a.Applicationgender,
                    ApplicationQualification = a.Applicationqualification, 
                    ApplicationMartialStatus = a.ApplicationMartialStatus,
                    ApplicationMobile = a.Applicationmobile,
                    ApplicationEmail = a.Applicationemail,
                    ApplicationRequestedAmount = a.ApplicationRequestedAmount,
                    ApplicationHobbies = a.ApplicationHobbies.Split(',', StringSplitOptions.RemoveEmptyEntries),
                    ApplicationRegisterDate = a.ApplicationRegisterdate,
                    ApplicationAddress = a.ApplicationAddress,
                    ApplicationDistrictId = a.ApplicationDistrictId,
                    ApplicationStateId = a.ApplicationStateId,
                    ApplicationCountryId = a.ApplicationCountryId,  
                    ApplicationIsAcceptedTermsAndConditions = a.ApplicationIsAcceptedTermsandConditions,
                    ApplicationDocumentTypeId = a.ApplicationDocumentUploads.DocumentTypeId
                }).SingleOrDefaultAsync();

            return applicantdetails;    
        }
        public async Task<List<ApplicationRegisterModel>> GetAppraisalsList()
        {
           /* var documenttypes = await _commonRepository.GetDocumentTypes();
            var statetypes = await _context.States.Select(a=>new IdNameModel() { Id =a.Id,Name = a.Name}).ToListAsync();
            var districttypes = await _context.Districts.Select(a => new IdNameModel() { Id = a.Id, Name = a.Name }).ToListAsync();
            var countrytypes = await _context.Countries.Select(a => new IdNameModel() { Id = a.Id, Name = a.Name }).ToListAsync();*/
            var result = await _context.ApplicationRegister.Include(a => a.ApplicationDocumentUploads).Where(a => a.Rowstate < 3).Select(a => new ApplicationRegisterModel()
            {
                Id = a.Id,
                ApplicationName = a.ApplicationName,
                ApplicationFatherName = a.Applicationfathername,
                ApplicationMotherName = a.Applicationmothername,
                ApplicationDob = a.Applicationdob,
                ApplicationGender = a.Applicationgender,
                ApplicationQualification = a.Applicationqualification,
                ApplicationMartialStatus = a.ApplicationMartialStatus,
                ApplicationMobile = a.Applicationmobile,
                ApplicationEmail = a.Applicationemail,
                ApplicationDocumentTypeId = a.ApplicationDocumentUploads != null ? a.ApplicationDocumentUploads.DocumentTypeId : null,
                ApplicationRequestedAmount = a.ApplicationRequestedAmount,
                ApplicationCountryId = a.ApplicationCountryId,
                ApplicationCountryName = a.Countries.Name,
                ApplicationStateId = a.ApplicationStateId,
                ApplicationDistrictId = a.ApplicationDistrictId,
                ApplicationIsAcceptedTermsAndConditions = a.ApplicationIsAcceptedTermsandConditions,
                ApplicationAddress = a.ApplicationAddress,
                ApplicationHobbies = a.ApplicationHobbies.Split(',', StringSplitOptions.RemoveEmptyEntries),
                ApplicationRegisterDate = a.ApplicationRegisterdate,
               // ApplicantDocumentTypes = documenttypes,
                ApplicationDocumentUploadModel = a.ApplicationDocumentUploads != null ? new ApplicationDocumentUploadModel() { DocumnentUploadId = a.ApplicationDocumentUploads.Id, DocumentName = a.ApplicationDocumentUploads.DocumentName, DocumentTypeId = a.ApplicationDocumentUploads.DocumentTypeId } : null,
                /*CountryTypes = countrytypes,
                StateTypes = statetypes,
                DistrictTypes = districttypes*/
            }).ToListAsync();
            return result;
        }
        public async Task<FileDownloadResult> ViewOrDownload(long id)
        {

            string[] supportedExtensions = new[] { ".jpg", ".pdf" };

            if (_env.IsProduction())
            {
                // 🟦 Azure Blob Storage
                var blobServiceClient = new BlobServiceClient(_appSettings.Value.AzureBlobConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_appSettings.Value.AzureBlobContainer);

                await containerClient.CreateIfNotExistsAsync();

                foreach (string extension in supportedExtensions)
                {
                    string blobName = $"{id}{extension}";
                    var blobClient = containerClient.GetBlobClient(blobName);

                    if (await blobClient.ExistsAsync())
                    {
                        var download = await blobClient.DownloadAsync();

                        var memoryStream = new MemoryStream();
                        await download.Value.Content.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        var contentType = _commonRepository.GetContentType(blobName);
                        if (string.IsNullOrEmpty(contentType))
                            contentType = "application/octet-stream";

                        return new FileDownloadResult
                        {
                            FileStream = memoryStream,
                            FileContent = contentType,
                            FileName = $"download{extension}",
                            IsSuccess = true,
                            Message = ""
                        };
                    }
                }

                return new FileDownloadResult { IsSuccess = false, Message = "File not found in Azure Blob." };
            }
            else
            {
                // 🟩 Local File System
                string documentFolderPath = Path.Combine(_appSettings.Value.ImagesPath, "NationalBanksDocuments", id.ToString());

                if (!Directory.Exists(documentFolderPath))
                {
                    return new FileDownloadResult { IsSuccess = false, Message = "Local file directory not found." };
                }

                string[] files = Directory.GetFiles(documentFolderPath, id.ToString() + ".*");

                if (files.Length == 1)
                {
                    string filePath = files[0];
                    var contentType = _commonRepository.GetContentType(filePath);

                    if (string.IsNullOrEmpty(contentType))
                        contentType = "application/octet-stream";

                    var memoryStream = new MemoryStream();
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await fs.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    }

                    return new FileDownloadResult
                    {
                        FileStream = memoryStream,
                        FileContent = contentType,
                        IsSuccess = true,
                        FileName = $"download{Path.GetExtension(filePath)}",
                        Message = ""
                    };
                }
                else
                {
                    return new FileDownloadResult { IsSuccess = false, Message = "Local file not found." };
                }
            }

        }
        

        public async Task<FileDownloadWithByteArrayResult> ReadFileAsync(long id, long documentid)
        {

            string uploadsdocumentPath = Path.Combine(_appSettings.Value.ImagesPath, "NationalBanksDocuments", id.ToString());

            if (string.IsNullOrEmpty(uploadsdocumentPath) || !Directory.Exists(uploadsdocumentPath))
            {
                return new FileDownloadWithByteArrayResult() { IsSuccess = false, Message = "File not found." };
            }

            string[] files = Directory.GetFiles(uploadsdocumentPath, id.ToString() + ".*");

            if (files.Length == 1)
            {
                string filePath = files[0];
                var contentType = _commonRepository.GetContentType(filePath);

                // Check if content type is null or empty
                if (string.IsNullOrEmpty(contentType))
                {
                    return new FileDownloadWithByteArrayResult() { IsSuccess = false, Message = "File not found." };
                }
                var memoryStream = new MemoryStream();
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    await fs.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    return new FileDownloadWithByteArrayResult()
                    {
                        FileBytes = memoryStream.ToArray(),
                        FileContent = contentType,
                        FileName = $"{id + Path.GetExtension(filePath).ToLowerInvariant()}",
                        IsSuccess = true,
                        Message = ""
                    };
                }
            }
            else if (files.Length > 1)
            {
                // Handle the case where multiple files are found
                return new FileDownloadWithByteArrayResult() { IsSuccess = false, Message = "File not found." };
            }
            else
            {
                // Handle the case where no file is found
                return new FileDownloadWithByteArrayResult() { IsSuccess = false, Message = "File not found." };
            }
        }

        public async Task<BaseResultModel> Saveappraisallist(List<ApplicationRegisterModel> models)
        {
            // Create a list to hold entities to be added or updated
            List<ApplicationRegister> applicationRegistersToAddOrUpdate = new List<ApplicationRegister>();

            try
            {
                foreach (var model in models)
                {
                    var applicationregister = await _context.ApplicationRegister.Include(a => a.ApplicationDocumentUploads).Where(b => b.Rowstate < 3 && b.Id == model.Id).FirstOrDefaultAsync();

                    if (applicationregister == null)
                    {
                        applicationregister = new ApplicationRegister();

                    }
                    if (applicationregister.ApplicationDocumentUploads == null)
                    {
                        applicationregister.ApplicationDocumentUploads =  new ApplicationDocumentUploads();

                    }
                    applicationregister.Id = model.Id;
                    applicationregister.ApplicationName = model.ApplicationName;
                    applicationregister.Applicationfathername = model.ApplicationFatherName;
                    applicationregister.Applicationmothername = model.ApplicationMotherName;
                    applicationregister.Applicationdob = model.ApplicationDob;
                    applicationregister.Applicationgender = model.ApplicationGender;
                    applicationregister.Applicationqualification = model.ApplicationQualification;
                    applicationregister.ApplicationMartialStatus = model.ApplicationMartialStatus;
                    applicationregister.Applicationmobile = model.ApplicationMobile;
                    applicationregister.Applicationemail = model.ApplicationEmail;
                    applicationregister.ApplicationRequestedAmount = model.ApplicationRequestedAmount;
                    applicationregister.ApplicationCountryId = model.ApplicationCountryId;
                    applicationregister.ApplicationStateId = model.ApplicationStateId;
                    applicationregister.ApplicationDistrictId = model.ApplicationDistrictId;
                    applicationregister.ApplicationIsAcceptedTermsandConditions = model.ApplicationIsAcceptedTermsAndConditions;
                    applicationregister.ApplicationAddress = model.ApplicationAddress;
                    applicationregister.ApplicationHobbies = string.Join(",", model.ApplicationHobbies);
                    applicationregister.ApplicationRegisterdate = model.ApplicationRegisterDate;
                    applicationregister.ApplicationStatus = true;

                    if (model.DocumentFile != null)
                    {
                        applicationregister.ApplicationDocumentUploads.DocumentName = model.DocumentFile.FileName;
                        applicationregister.ApplicationDocumentUploads.DocumentTypeId = 2;
                    }

                    applicationRegistersToAddOrUpdate.Add(applicationregister);
                }

                // Add the list of entities to the context
                _context.ApplicationRegister.UpdateRange(applicationRegistersToAddOrUpdate);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // After saving changes, iterate through the list to save files
                for (int index = 0; index < models.Count; index++)
                {
                    var model = models[index];
                    if (model.DocumentFile !=null)
                    {
                        string uploadsdocumentPath = Path.Combine(_appSettings.Value.ImagesPath, "NationalBanksDocuments", applicationRegistersToAddOrUpdate[index].Id.ToString());

                        if (!Directory.Exists(uploadsdocumentPath))
                        {
                            Directory.CreateDirectory(uploadsdocumentPath);
                        }

                        if (model != null && model.DocumentFile != null)
                        {
                            string panfilePath = "";
                            if (model.DocumentFile.ContentType == "image/jpeg")
                            {
                                panfilePath = Path.Combine(uploadsdocumentPath, $"{applicationRegistersToAddOrUpdate[index].ApplicationDocumentUploads.ApplicationId}.jpg");
                            }
                            else
                            {
                                panfilePath = Path.Combine(uploadsdocumentPath, $"{applicationRegistersToAddOrUpdate[index].ApplicationDocumentUploads.ApplicationId}.pdf");
                            }
                            if (model.Id > 0)
                            {
                                // If File Exists in same path then nedd to delete file before overriding 
                                string jpgFilePath = Path.Combine(uploadsdocumentPath, $"{applicationRegistersToAddOrUpdate[index].ApplicationDocumentUploads.ApplicationId}.jpg");
                                string pdfFilePath = Path.Combine(uploadsdocumentPath, $"{applicationRegistersToAddOrUpdate[index].ApplicationDocumentUploads.ApplicationId}.pdf");
                                if (File.Exists(jpgFilePath))
                                {
                                    File.Delete(jpgFilePath);
                                }
                                else if (File.Exists(pdfFilePath))
                                {
                                    File.Delete(pdfFilePath);
                                }

                            }
                            using (var fileStream = new FileStream(panfilePath, FileMode.Create))
                            {
                                await model.DocumentFile.CopyToAsync(fileStream);
                            }
                        }
                    }                    
                }

                return new BaseResultModel() { IsSuccess = true,Message = "Savew"};
            }
            catch(Exception ex)
            {
                return new BaseResultModel() { IsSuccess = false,Message=ex.Message};
            }
         

        }
        public async Task<BaseResultModel> Deletefromappraisallist(long id)
        {
            try
            {
                if (id > 0)
                {
                    var applicationregister = await _context.ApplicationRegister.Include(a => a.ApplicationDocumentUploads).Where(b => b.Rowstate < 3 && b.Id == id).FirstOrDefaultAsync();

                    if (applicationregister != null)
                    {
                        applicationregister.Rowstate = 3;
                        applicationregister.ApplicationDocumentUploads.Rowstate = 3;
                        _context.ApplicationRegister.Update(applicationregister);
                        await _context.SaveChangesAsync();

                        var blobServiceClient = new BlobServiceClient(_appSettings.Value.AzureBlobConnectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient(_appSettings.Value.AzureBlobContainer);
                        string applicationIdPrefix = applicationregister.ApplicationDocumentUploads.ApplicationId.ToString();
                        await containerClient.CreateIfNotExistsAsync();
                        // 🔍 Delete all existing blobs that start with the same ApplicationId
                        await foreach (var existingBlob in containerClient.GetBlobsAsync())
                        {
                            if (existingBlob.Name.StartsWith(applicationIdPrefix))
                            {
                                var existingBlobClient = containerClient.GetBlobClient(existingBlob.Name);
                                await existingBlobClient.DeleteIfExistsAsync();
                            }
                        }
                        return new BaseResultModel() { IsSuccess = true, Message = "Appraisal Deleted Sucessfully" };
                    }
                    else
                    {
                        return new BaseResultModel() { IsSuccess = false, Message = "Internal Server Error" };
                    }
                }
                else
                {
                    return new BaseResultModel() { IsSuccess = false, Message = "Internal Server Error" };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error removing entity: " + ex.Message);
                return new BaseResultModel() { IsSuccess = false, Message =ex.Message};
            }
        }
              
        
    }
}
