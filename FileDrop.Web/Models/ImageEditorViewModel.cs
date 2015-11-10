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
        public string DataUrl { get; set; }

        public ImageEditorViewModel(File file, string dataUrl)
        {
            File = file;
            DataUrl = dataUrl;
        }
    }
}