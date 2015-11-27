using Abp.Application.Navigation;

namespace FileDrop.Web.Models.Layout
{
    public class TopMenuViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}