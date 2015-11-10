using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using FileDrop.Domains;
using FileDrop.Interfaces;

namespace FileDrop.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<File> _fileRepository;

        public FileService(IRepository<File> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public ICollection<File> GetAllFiles()
        {
             var files = _fileRepository.GetAll().ToList();

            return files;
        }

        public File GetFileById(int id)
        {
            var file = _fileRepository.Get(id);
            return file;
        }

        public void SaveFile(File file)
        {
            if (file.Id == 0)
            {
                _fileRepository.Insert(file);
            }
            else
            {
                _fileRepository.Update(file);
            }
        }
    }
}
