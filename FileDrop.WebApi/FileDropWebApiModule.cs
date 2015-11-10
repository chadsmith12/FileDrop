using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace FileDrop
{
    [DependsOn(typeof(AbpWebApiModule), typeof(FileDropApplicationModule))]
    public class FileDropWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(FileDropApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
