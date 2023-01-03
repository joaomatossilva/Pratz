using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pratz.Web.Pages
{
    [BindProperties]
    public class ProfileModel : PageModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }


        public void OnGet()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                UserName = User.Identity.Name;
            }
        }

        public async Task<IActionResult> OnPost([FromQuery] string room)
        {
            var user = User;
            if(user?.Identity.IsAuthenticated == false)
            {
                user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme));
            }
            else
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    }, CookieAuthenticationDefaults.AuthenticationScheme));
            }

            //return SignIn(user, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(user);

            return !string.IsNullOrEmpty(room) ? RedirectToPage("Room", new {id = room}) : RedirectToPage("Index");
        }

    }
}