using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyDocDb.WebApplication.Models
{
    public class User
    {
        public User()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
