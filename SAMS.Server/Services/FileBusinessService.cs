using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.Data.Dtos;
using SAMS.DataAccess;
using SAMS.Server.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SAMS.Server.Services
{
    /// <summary>
    /// Dosya işlemleri servisi
    /// </summary>
    public class FileBusinessService : IFilesBusinessService
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Dosya işlemleri servisi ctor
        /// </summary>
        public FileBusinessService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Dosya yükleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<NewFileResponseDetailedDTO> UploadFile(string folderName, string referenceGuid, IFormFile file)
        {
            var documentRepo = unitOfWork.GetRepository<Document>();

            var filePath = string.Format("{0}\\Uploads\\{1}\\{2}", Path.Combine(Directory.GetCurrentDirectory()), folderName, referenceGuid);
            bool exists = Directory.Exists(filePath);
            if (!exists)
            {
                Directory.CreateDirectory(filePath);
            }

            using (FileStream stream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document()
            {
                Url = filePath,
                FolderName = folderName,
                FileContentType = file.ContentType,
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = file.FileName,
                FileSize = file.Length,
                ReferenceGuid = referenceGuid,
                DownloadLink = string.Format(AppSettingsHelper.Current.FileUploadPathName, folderName, referenceGuid) + "/" + file.FileName
            };
            await documentRepo.Add(document);
            await unitOfWork.SaveChangesAsync();

            return new NewFileResponseDetailedDTO
            {
                FileName = file.FileName,
                Uploaded = 1,
                Url = filePath
            };
        }

        /// <summary>
        /// Çoklu dosya yükleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        public async Task<List<NewFileResponseDetailedDTO>> UploadFiles(string folderName, string referenceGuid, IFormFileCollection formFiles)
        {
            var addedFiles = new List<NewFileResponseDetailedDTO>();
            foreach (var file in formFiles)
            {
                var result = await UploadFile(folderName, referenceGuid, file);
                addedFiles.Add(result);
            }

            return addedFiles;
        }

        /// <summary>
        /// Dosyaları listeleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <returns></returns>
        public async Task<List<ListFilesResponseDTO>> ListFiles(string folderName, string referenceGuid)
        {
            var files = await unitOfWork.GetRepository<Document>().GetAll(d => d.IsDeleted == false && d.FolderName == folderName && d.ReferenceGuid == referenceGuid);
            return files.Select(d => new ListFilesResponseDTO
            {
                Url = d.DownloadLink
            }).ToList();
        }

        /// <summary>
        /// Dosya silme
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public async Task<DeleteFileResponseDTO> DeleteFile(string fileUrl)
        {
            var deletedCount = 0;
            var documentRepo = unitOfWork.GetRepository<Document>();
            var document = await documentRepo.Get(d => d.IsDeleted == false && d.DownloadLink == fileUrl);
            if (document != null)
            {
                documentRepo.Delete(document);
                deletedCount = await unitOfWork.SaveChangesAsync();
                var fileFullUrl = string.Format("{0}\\Uploads\\{1}\\{2}", Path.Combine(Directory.GetCurrentDirectory()), document.FolderName, document.ReferenceGuid) + "\\" + document.FileName;
                if (File.Exists(fileFullUrl))
                {
                    File.Delete(fileFullUrl);
                }
            }
            
            return new DeleteFileResponseDTO
            {
                NumberOfDeletedObjects = deletedCount
            };
        }
    }
}