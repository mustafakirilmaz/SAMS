using SAMS.Data.Dtos;
using SAMS.Infrastructure.Controller;
using SAMS.Server.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAMS.Server.Controllers
{
    /// <summary>
    /// Dosya işlemleri
    /// </summary>
    [Route("api/files")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IFilesBusinessService filesService;

        /// <summary>
        /// Dosya işlemleri ctor
        /// </summary>
        /// <param name="filesService"></param>
        public FilesController(IFilesBusinessService filesService)
        {
            this.filesService = filesService;
        }

        /// <summary>
        /// Dosya yükleme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="formFiles"></param>
        /// <param name="referenceGuid"></param>
        /// <returns></returns>
        [HttpPost("{folderName}/add/{referenceGuid}")]
        public async Task<ActionResult<NewFileResponseDTO>> UploadFiles(string folderName, IFormFileCollection formFiles, string referenceGuid)
        {
            if (formFiles == null)
            {
                return BadRequest("Yüklenecek dosya bulunamadı.");
            }
            if (formFiles.Count == 0)
            {
                formFiles = HttpContext.Request.Form.Files;
            }

            var response = await filesService.UploadFiles(folderName, referenceGuid, formFiles);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        /// <summary>
        /// Klasördeki dosyaları getirme
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="referenceGuid"></param>
        /// <returns></returns>
        [HttpGet("{folderName}/list/{referenceGuid}")]
        public async Task<ActionResult<IEnumerable<ListFilesResponseDTO>>> GetFilesByFolderName(string folderName, string referenceGuid)
        {
            var response = await filesService.ListFiles(folderName, referenceGuid);

            return Ok(response);
        }

        /// <summary>
        /// Dosya silme
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<DeleteFileResponseDTO>> DeleteFile(string fileUrl)
        {
            var response = await filesService.DeleteFile(fileUrl);

            return Ok(response);
        }
    }
}
