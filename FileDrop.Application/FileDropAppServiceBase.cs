using Abp.Application.Services;

namespace FileDrop
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class FileDropAppServiceBase : ApplicationService
    {
        protected FileDropAppServiceBase()
        {
            LocalizationSourceName = FileDropConsts.LocalizationSourceName;
        }
    }
}