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

        public ICollection<File> GetAllFiles(string searchTerm, bool filter)
        {
            var files = _fileRepository.GetAll();

            // no search term and filter, just return all the files
            if (searchTerm.IsNullOrWhiteSpace() && !filter)
                return files.ToList();
            if (searchTerm.IsNullOrWhiteSpace() && filter)
                return files.Where(x => x.IsImage).ToList();

            // search term, limit it to the search
            if (filter)
            {
                files = files.Where(x => x.FileName.Contains(searchTerm) && x.IsImage);
                return files.ToList();
            }
            files = files.Where(x => x.FileName.Contains(searchTerm) && x.IsImage == filter);
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
