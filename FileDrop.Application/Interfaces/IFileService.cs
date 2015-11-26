using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// <param name="filter"></param>
        /// <returns></returns>
        ICollection<File> GetAllFiles(string searchTerm, bool filter);

        /// <summary>
        /// Gets all files for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="filter">if set to <c>true</c> [filter].</param>
        /// <returns></returns>
        ICollection<File> GetAllFilesForUser(long userId, string searchTerm, bool filter);

            /// <summary>
        /// Gets the file by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        File GetFileById(int id);

        /// <summary>
        /// Gets the file by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<File> GetFileByIdAsync(int id);

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="file">The file.</param>
        void SaveFile(File file);

        /// <summary>
        /// Saves the file asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        Task SaveFileAsync(File file);
    }
}
