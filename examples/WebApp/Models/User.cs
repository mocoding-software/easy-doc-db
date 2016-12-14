using System;

namespace EasyDocDb.WebApplication.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}