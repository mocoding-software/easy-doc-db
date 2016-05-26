using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Mocoding.EasyDocDb.CrudSample.ViewModels.Manage
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public List<AuthenticationDescription> OtherLogins { get; set; }
    }
}
