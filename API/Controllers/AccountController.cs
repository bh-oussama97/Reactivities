using API.DTO;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> usermanager;
        private readonly SignInManager<User> siginInmanager;
        private readonly TokenService tokenservice;

        public AccountController(UserManager<User> usermanager,SignInManager<User> siginInmanager,TokenService tokenservice)
        {
            this.usermanager = usermanager;
            this.siginInmanager = siginInmanager;
            this.tokenservice = tokenservice;
        }


        [HttpPost("login")]
        

        public async Task<ActionResult<UserDTO>> Login(LoginDTO logindto)
        {

            var user = await usermanager.FindByEmailAsync(logindto.Email);

            if (user == null) return Unauthorized();

            var result = await siginInmanager.CheckPasswordSignInAsync(user, logindto.Password, false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();

        }



        [HttpPost("register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerdto)
        {
            if ( usermanager.Users.Any(x => x.Email == registerdto.Email))
            {
                return BadRequest("Email Taken");
            }
           

                 if ( usermanager.Users.Any(x => x.UserName == registerdto.Username))
            {
                return BadRequest("Username Taken");
            }

            var user = new User
            {
                DisplayName = registerdto.DisplayName,
                Email = registerdto.Email,
                UserName = registerdto.Username
            };

            var result = await usermanager.CreateAsync(user, registerdto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user");
        }

        //user should be authenticed
        [Authorize]
        [HttpGet("CurrentUser")]

        public async Task<ActionResult<UserDTO>> getCurrentUser()
        {

            //get the current user by the email stored in token 
            var user = await usermanager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }


        private UserDTO CreateUserObject(User user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = tokenservice.createToken(user),
                Username = user.UserName
            };
        }
    }
}
