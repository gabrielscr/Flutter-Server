using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Server.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string UserLogin { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Age { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public bool? Active { get; set; }

        public string ProfileImage { get; set; }
    }
}
