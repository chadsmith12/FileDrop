using Abp.Web.Mvc.Controllers;

namespace FileDrop.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class FileDropControllerBase : AbpController
    {
        protected FileDropControllerBase()
        {
            LocalizationSourceName = FileDropConsts.LocalizationSourceName;
        }
    }
}