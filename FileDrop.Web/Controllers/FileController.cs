using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.UI;
using FileDrop.Interfaces;
using FileDrop.Web.Helpers;
using FileDrop.Web.Models;
using File = FileDrop.Domains.File;

namespace FileDrop.Web.Controllers
{
    public class FileController : FileDropControllerBase
    {
        #region Private Fields
        private readonly IFileService _fileService;
        #endregion

        #region Constructors

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion

        #region Public Methods

        public PartialViewResult GetAllFiles(string searchTerm)
        {
            var files = _fileService.GetAllFiles(searchTerm);
            var model = new FilesViewModel {Files = files.ToList()};

            return PartialView("_FilesTable", model);
        }

        public ActionResult Index()
        {
            var files = _fileService.GetAllFiles(string.Empty);
            var model = new FilesViewModel {Files = files.ToList()};

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateFileName(int id, string fileName)
        {
            var file = _fileService.GetFileById(id);
            file.FileName = fileName;

            _fileService.SaveFile(file);

            return Json(new {saved = true});
        }

        public ActionResult UploadFile()
        {
            string fileName = "";

            try
            {
                foreach (HttpPostedFileBase file in Request.Files.Cast<string>().Select(fileItem => Request.Files[fileItem]).Where(file => file != null))
                {
                    fileName = file.FileName;
                    var fileSize = file.ContentLength;

                    // check to make sure we actually have a file
                    if (file.ContentLength <= 0) throw new UserFriendlyException("I'm sorry, there was an error saving the file.");

                    // read in the file into a byte array
                    byte[] binaryFile;
                    using (var binaryReady = new BinaryReader(file.InputStream))
                    {
                        binaryFile = binaryReady.ReadBytes(file.ContentLength);
                    }

                    var fullPath = FileHelpers.GetPath(fileName, Server.MapPath(@"\App_Data\"));
                    var saveFile = new File
                    {
                        FileName = fileName.Split('.')[0], // don't save the file name with the extension
                        FileSize = FileHelpers.BytesToMegaBytes(fileSize),
                        FileType = file.ContentType,
                        FilePath = fullPath,
                        UploadDateTime = DateTime.Now,
                        IsImage = FileHelpers.IsImage(file.ContentType)
                    };

                    FileHelpers.EncryptFileToDisk(binaryFile, fullPath, "zxcvbgfdsaqwert54321");
                    _fileService.SaveFile(saveFile);
                }
                
            }
            catch (Exception ex)
            {
                // throw the user friendly exception so the user sees the error
                throw new UserFriendlyException("I'm sorry, there was an error saving the file.");
            }

            return Json(new {message = fileName});

        }

        public ActionResult EditImage(int id)
        {
            var file = _fileService.GetFileById(id);
            var data = FileHelpers.ReadAllBytes(file.FilePath);
            var fileDecrypted = FileHelpers.Decrypt(data, "zxcvbgfdsaqwert54321");
            var base64 = Convert.ToBase64String(fileDecrypted);
            var dataUrl = FileHelpers.ToDataUrl(base64, file.FileType);

            var model = new ImageEditorViewModel(file, dataUrl);
            return View("ImageEditor", model);
        }

        public void DownloadFile(int id)
        {
            var file = _fileService.GetFileById(id);
            var data = FileHelpers.ReadAllBytes(file.FilePath);
            var fileDecrypted = FileHelpers.Decrypt(data, "zxcvbgfdsaqwert54321");

            Response.ContentType = MimeMapping.GetMimeMapping(file.FilePath);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + Path.GetFileName(file.FilePath)));
            Response.OutputStream.Write(fileDecrypted, 0, fileDecrypted.Length);
            Response.Flush();
        }

        #endregion
    }
}
