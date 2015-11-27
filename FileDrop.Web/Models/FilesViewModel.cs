using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDrop.Domains;

namespace FileDrop.Web.Models
{
    public class FilesViewModel
    {
        public List<File> Files { get; set; }
        public long UserId { get; set; }

        public FilesViewModel()
        {
            Files = new List<File>();
        }

        public FilesViewModel(List<File> files, long userId)
        {
            Files = files;
            UserId = userId;
        }
    }
}