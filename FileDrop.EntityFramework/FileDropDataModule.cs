using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using FileDrop.EntityFramework;

namespace FileDrop
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(FileDropCoreModule))]
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
