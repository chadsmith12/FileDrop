using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Extensions;
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

        public ICollection<File> GetAllFiles(string searchTerm)
        {
            var files = _fileRepository.GetAll();

            // no search term, just return all the files
            if (searchTerm.IsNullOrWhiteSpace())
                return files.ToList();

            // search term, limit it to the search, then return the files
            files = files.Where(x => x.FileName.Contains(searchTerm));
            return files.ToList();
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
