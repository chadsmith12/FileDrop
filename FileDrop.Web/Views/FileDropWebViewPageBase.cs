using Abp.Web.Mvc.Views;

namespace FileDrop.Web.Views
{
    public abstract class FileDropWebViewPageBase : FileDropWebViewPageBase<dynamic>
    {

    }

    public abstract class FileDropWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected FileDropWebViewPageBase()
        {
            LocalizationSourceName = FileDropConsts.LocalizationSourceName;
        }
    }
}