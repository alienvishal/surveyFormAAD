using eXtolloURLWhitelist.Models;
using eXtolloURLWhitelist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Controllers
{
    [Authorize]
    public class Survey:Controller
    {
        private readonly ILogger<Survey> logger;
        private readonly ISurveyRepository surveyRepository;

        public Survey(ILogger<Survey> logger, ISurveyRepository surveyRepository)
        {
            this.logger = logger;
            this.surveyRepository = surveyRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            SurveyViewModel model = new SurveyViewModel
            {
                Questions = surveyRepository.GetAllQuestion(),
                UserId = User.Identity.Name
            };
            ViewBag.ProjectInstance = surveyRepository.GetAllProjectInstance();
            return View(model);
        }
    }
}
