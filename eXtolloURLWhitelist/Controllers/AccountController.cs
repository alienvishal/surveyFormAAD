using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Controllers
{
    public class AccountController:Controller
    {
        private readonly IAuthorizationService authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        public IActionResult CheckSignInAsync()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            var callbackUrl = "https://extollosurvey-corpdir.msappproxy.net/?appproxy=logout";
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
