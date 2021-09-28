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
    public class SurveyController:Controller
    {
        private readonly ILogger<SurveyController> logger;
        private readonly ISurveyRepository surveyRepository;

        public SurveyController(ILogger<SurveyController> logger, ISurveyRepository surveyRepository)
        {
            this.logger = logger;
            this.surveyRepository = surveyRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.Name == "vishal@apac.corpdir.net")
            {
                return RedirectToAction("AddQuestion", "Admin");
            }
            SurveyViewModel model = new SurveyViewModel
            {
                Questions = surveyRepository.GetAllQuestion(),
                UserId = User.Identity.Name
            };
            ViewBag.ProjectInstance = surveyRepository.GetAllProjectInstance();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SurveyViewModel model)
        {
            if (User.Identity.Name == "vishal@apac.corpdir.net")
            {
                return RedirectToAction("AddQuestion", "Admin");
            }

            if (ModelState.IsValid)
            {
                bool isSurveyTaken = surveyRepository.IsSurveyTaken(model);
                if (isSurveyTaken)
                {
                    ViewBag.AlreadySubmitted = "Thank you for your time, The feedback is already been submitted for selected instance";
                }
                else
                {
                    bool isSurveyAdded = surveyRepository.IsSurveyAdded(model);
                    if (isSurveyAdded)
                    {
                        ViewBag.Success = "Thank you for your time, feedback has been submitted Successfully";
                    }
                    else
                    {
                        ViewBag.Error = "Something Went Wrong";
                    }
                }
            }
            SurveyViewModel surveyViewModel = new SurveyViewModel
            {
                Questions = surveyRepository.GetAllQuestion(),
                UserId = User.Identity.Name
            };
            ViewBag.ProjectInstance = surveyRepository.GetAllProjectInstance();
            return View(surveyViewModel);
        }

    }
}
