using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mocoding.EasyDocDb.CrudSample.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<string>
    {             
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
