using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{

    //UserDTO is a class that will returned after the user logins in as json response
    public class UserDTO
    {

        public string DisplayName { get; set; }

        public string Token { get; set; }


        public string Username { get; set; }

        public string Image { get; set; }


    }
}
