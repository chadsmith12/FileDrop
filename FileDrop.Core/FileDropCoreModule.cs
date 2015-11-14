using System.Reflection;
using Abp.Modules;
using Abp.Zero;

namespace FileDrop
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class FileDropCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
