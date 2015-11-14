using System.Data.Entity;
using Abp.EntityFramework;
using Abp.Zero.EntityFramework;
using FileDrop.Domains;

namespace FileDrop.EntityFramework
{
    public class FileDropDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for each Entity...
        public virtual IDbSet<File> Files { get; set; } 

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public FileDropDbContext()
            : base("FileDrop")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in FileDropDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of FileDropDbContext since ABP automatically handles it.
         */
        public FileDropDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
