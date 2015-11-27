using System.Collections.Generic;
using Abp.Localization;

namespace FileDrop.Web.Models.Layout
{
    public class UserInfoViewModel
    {
        public bool IsLoggedIn { get; set; }

        public string UserName { get; set; }
    }
}