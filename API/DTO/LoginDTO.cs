using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{

    //login data transfer objecs is a class that will transfer objects of users to login in
    public class LoginDTO
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

    }
}
