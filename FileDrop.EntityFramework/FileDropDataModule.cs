using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using FileDrop.EntityFramework;

namespace FileDrop
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(FileDropCoreModule))]
    public class FileDropDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "FileDrop";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<FileDropDbContext>(null);
        }
    }
}
