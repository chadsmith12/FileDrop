using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using FileDrop.Interfaces;
using FileDrop.Web.Helpers;
using FileDrop.Web.Models;
using File = FileDrop.Domains.File;

namespace FileDrop.Web.Controllers
{
    [AbpMvcAuthorize]
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

        public PartialViewResult GetAllFiles(string searchTerm, bool filter)
        {
            var userId = AbpSession.GetUserId();
            var files = _fileService.GetAllFilesForUser(userId, searchTerm, filter);
            var model = new FilesViewModel {Files = files.ToList()};

            return PartialView("_FilesTable", model);
        }

        public ActionResult Index()
        {
            var userId = AbpSession.GetUserId();
            var files = _fileService.GetAllFilesForUser(userId, string.Empty, false).ToList();
            var model = new FilesViewModel(files, userId);

            ViewBag.IsLoggedIn = true;

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateFileName(int id, string fileName)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            file.FileName = fileName;

            await _fileService.SaveFileAsync(file);

            return Json(new {saved = true});
        }

        public async Task<ActionResult> UploadFile()
        {
            string fileName = "";
            var userId = AbpSession.GetUserId();

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

                    var fullPath = FileHelpers.GetPath(fileName, Server.MapPath(@"\App_Data\"), userId);
                    var saveFile = new File
                    {
                        FileName = fileName, // don't save the file name with the extension
                        FileSize = FileHelpers.BytesToMegaBytes(fileSize),
                        FileType = file.ContentType,
                        FilePath = fullPath,
                        UploadDateTime = DateTime.Now,
                        IsImage = FileHelpers.IsImage(file.ContentType),
                        UserId = userId
                    };

                    await FileHelpers.EncryptFileToDiskAsync(binaryFile, fullPath, "zxcvbgfdsaqwert54321");
                    await _fileService.SaveFileAsync(saveFile);
                }
                
            }
            catch (Exception ex)
            {
                // throw the user friendly exception so the user sees the error
                throw new UserFriendlyException("I'm sorry, there was an error saving the file.");
            }

            return Json(new {message = fileName});

        }

        [HttpPost]
        public async Task<ActionResult> SaveEditedImage(ImageEditorViewModel imageEditorViewModel)
        {
            var userId = AbpSession.GetUserId();
            var file = await _fileService.GetFileByIdAsync(imageEditorViewModel.FileId);
            // make a regular expression to get the type and image data quickly
            var imageRegex = Regex.Match(imageEditorViewModel.DataUrl, @"data:image/(?<type>.+?),(?<data>.+)");
            var imageType = imageRegex.Groups["type"].Value.Split(';')[0];
            var base64Data = imageRegex.Groups["data"].Value;
            var binaryData = Convert.FromBase64String(base64Data);
            var size = binaryData.Length;
            string path;

            // saving as a new file
            if (file.Id == 0)
            {
                imageEditorViewModel.FileName += "." + imageType;
                path = FileHelpers.GetPath(imageEditorViewModel.FileName, Server.MapPath(@"\App_Data\"), userId);
            }
            else
            {
                path = file.FilePath;
            }

            await FileHelpers.EncryptFileToDiskAsync(binaryData, path, "zxcvbgfdsaqwert54321");

            // go ahead and return if we are not making a new file
            if (file.Id != 0) return Json(new {fileName = file.FileName});

            var newFile = new File
            {
                FileName = imageEditorViewModel.FileName,
                FilePath = path,
                FileSize = FileHelpers.BytesToMegaBytes(size),
                UploadDateTime = DateTime.Now,
                FileType = "image/" + imageType,
                IsImage = true,
                UserId = userId
            };

           await _fileService.SaveFileAsync(newFile);
            return Json(new {fileName = imageEditorViewModel.FileName});
        }

        public async Task<ActionResult> EditImage(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            var data = await FileHelpers.ReadAllBytesAsync(file.FilePath);
            var fileDecrypted = await FileHelpers.DecryptAsync(data, "zxcvbgfdsaqwert54321");
            var base64 = Convert.ToBase64String(fileDecrypted);
            var dataUrl = FileHelpers.ToDataUrl(base64, file.FileType);

            var model = new ImageEditorViewModel(file, dataUrl);
            return View("ImageEditor", model);
        }

        public async Task DownloadFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            var data = await FileHelpers.ReadAllBytesAsync(file.FilePath);
            var fileDecrypted = await FileHelpers.DecryptAsync(data, "zxcvbgfdsaqwert54321");

            Response.ContentType = MimeMapping.GetMimeMapping(file.FilePath);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + Path.GetFileName(file.FilePath)));
            await Response.OutputStream.WriteAsync(fileDecrypted, 0, fileDecrypted.Length);
            Response.Flush();
        }

        public async Task DownloadArchive()
        {
            var userId = AbpSession.GetUserId();
            var files = _fileService.GetAllFilesForUser(userId, string.Empty, false);
            var orginalDirectory =
                        new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\App_Data\")));
            var path = Path.Combine(orginalDirectory.ToString(), userId.ToString());
            var exists = Directory.Exists(path);
            if (!exists)
            {
                Directory.CreateDirectory(path);
            }

            // get all files ready to be archived
            foreach (var file in files)
            {
                // decrypt the file and write the temp file
                var data = await FileHelpers.ReadAllBytesAsync(file.FilePath);
                var fileDecrypted = await FileHelpers.DecryptAsync(data, "zxcvbgfdsaqwert54321");
                var fullPath = string.Format("{0}\\{1}", path, file.FileName);
                System.IO.File.WriteAllBytes(fullPath, fileDecrypted);
            }

            // archive the files
            var archivePath = new DirectoryInfo(string.Format("{0}Archives", Server.MapPath(@"\App_Data\")));
            exists = Directory.Exists(archivePath.ToString());
            if (!exists)
            {
                Directory.CreateDirectory(archivePath.ToString());
            }
            var archive = Path.Combine(archivePath.ToString(), DateTime.Now.Ticks + ".zip");
            ZipFile.CreateFromDirectory(path, archive, CompressionLevel.Fastest, true);

            var zipData = await FileHelpers.ReadAllBytesAsync(archive);
            Response.ContentType = MimeMapping.GetMimeMapping(archive);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + Path.GetFileName(archive)));
            await Response.OutputStream.WriteAsync(zipData, 0, zipData.Length);
            Response.Flush();
        }

        #endregion
    }
}
