using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Controllers
{
    public class ErrorController:Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCode(int StatusCode)
        {
            if(StatusCode == 404)
            {
                return View("PageNotFound");
            }
            return View("InternalServerError");
        }

        [Route("Error")]
        public IActionResult ExceptionError()
        {
            return View("InternalServerError");
        }
    }
}
