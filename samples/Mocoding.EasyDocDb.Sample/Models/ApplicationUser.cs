using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mocoding.EasyDocDb.Sample.Models
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
