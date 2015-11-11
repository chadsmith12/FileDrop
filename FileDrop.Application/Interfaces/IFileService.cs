using System.Collections;
using System.Collections.Generic;
using Abp.Application.Services;
using FileDrop.Domains;

namespace FileDrop.Interfaces
{
    public interface IFileService : IApplicationService
    {
        /// <summary>
        /// Gets all files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        ICollection<File> GetAllFiles(string searchTerm);

        /// <summary>
        /// Gets the file by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        File GetFileById(int id);

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="file">The file.</param>
        void SaveFile(File file);
    }
}
