using System.Web;
using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Localization;
using Abp.Threading;
using FileDrop.Managers;
using FileDrop.Web.Models.Layout;
using Microsoft.Owin.Security;

namespace FileDrop.Web.Controllers
{
    public class LayoutController : FileDropControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly UserManager _userManager;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public LayoutController(IUserNavigationManager userNavigationManager, ILocalizationManager localizationManager, UserManager userManager)
        {
            _userNavigationManager = userNavigationManager;
            _localizationManager = localizationManager;
            _userManager = userManager;
        }

        [ChildActionOnly]
        public PartialViewResult TopMenu(string activeMenu = "")
        {
            var isLoggedIn = AbpSession.UserId.HasValue;
            var model = new TopMenuViewModel
                        {
                            MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.UserId)),
                            ActiveMenuItemName = activeMenu,
                            IsLoggedIn = isLoggedIn
                        };

            return PartialView("_TopMenu", model);
        }

        [ChildActionOnly]
        public PartialViewResult UserInfoBar()
        {
            var userId = AbpSession.UserId;
            string userName = string.Empty;

            if (userId.HasValue)
            {
                var user = AsyncHelper.RunSync(() =>_userManager.FindByIdAsync(userId.Value));
                userName = user.UserName;
            }

            var model = new UserInfoViewModel
                        {
                            UserName = userName,
                            IsLoggedIn = userId.HasValue,
                        };

            return PartialView("_UserInfoBar", model);
        }
    }
}