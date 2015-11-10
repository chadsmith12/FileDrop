using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace FileDrop.Domains
{
    public class File : Entity
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public virtual string FileType { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public virtual double FileSize { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is image; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsImage { get; set; }

        /// <summary>
        /// Gets or sets the upload date time.
        /// </summary>
        /// <value>
        /// The upload date time.
        /// </value>
        public virtual DateTime UploadDateTime { get; set; }
    }
}
