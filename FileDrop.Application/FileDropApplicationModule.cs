using System.Reflection;
using Abp.Modules;

namespace FileDrop
{
    [DependsOn(typeof(FileDropCoreModule))]
    public class FileDropApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
