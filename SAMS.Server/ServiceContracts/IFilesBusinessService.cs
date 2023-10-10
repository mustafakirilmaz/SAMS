using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SAMS.Data.Dtos;

namespace SAMS.Server.ServiceContracts
{
    /// <summary>
    /// Dosya işlemleri servisi
    /// </summary>
    public interface IFilesBusinessService
    {
        /// <summary>
        /// Dosya yükleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<NewFileResponseDetailedDTO> UploadFile(string folderName, string referenceGuid, IFormFile file);

        /// <summary>
        /// Çoklu dosya yükleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        Task<List<NewFileResponseDetailedDTO>> UploadFiles(string folderName, string referenceGuid, IFormFileCollection formFiles);

        /// <summary>
        /// Dosyaları listeleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <returns></returns>
        Task<List<ListFilesResponseDTO>> ListFiles(string folderName, string referenceGuid);

        /// <summary>
        /// Dosya silme
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        Task<DeleteFileResponseDTO> DeleteFile(string fileUrl);

    }
}
