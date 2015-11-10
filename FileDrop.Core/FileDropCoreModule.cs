using System.Reflection;
using Abp.Modules;

namespace FileDrop
{
    public class FileDropCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
