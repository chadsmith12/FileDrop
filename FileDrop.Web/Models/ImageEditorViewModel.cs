using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDrop.Domains;

namespace FileDrop.Web.Models
{
    public class ImageEditorViewModel
    {
        public File File { get; set; }
        public int FileId { get; set; }
        public string DataUrl { get; set; }
        public string FileName { get; set; }
        public byte[] BinaryFile { get; set; }

        public ImageEditorViewModel()
        {
            File = new File();
            FileId = 0;
            DataUrl = string.Empty;
            FileName = string.Empty;
        }

        public ImageEditorViewModel(File file, string dataUrl)
        {
            File = file;
            DataUrl = dataUrl;
        }
    }
}