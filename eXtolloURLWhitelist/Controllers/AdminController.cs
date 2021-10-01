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
    public class AdminController:Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly ISurveyRepository surveyRepository;
        private string[] Users = new string[] { "vishal@apac.corpdir.net", "kasridh@apac.corpdir.net", "nper@emea.corpdir.net", "maikewi@emea.corpdir.net" };

        public AdminController(ILogger<AdminController> _logger, ISurveyRepository surveyRepository)
        {
            logger = _logger;
            this.surveyRepository = surveyRepository;
        }
        [HttpGet]
        public IActionResult AddQuestion()
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuestion(AddQuestionViewModel model)
        {
            if(! Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            bool isQuestionAdded = surveyRepository.IsQuestionAdded(model);
            if (isQuestionAdded)
            {
                ViewBag.QuestionAdded = "The URL has been Added";
                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Could not able to add URL");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListQuestion()
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            return View(surveyRepository.GetAllQuestion());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            var question = surveyRepository.GetQuestionById(id);

            if (question == null)
            {
                ViewBag.Error = $"The id {question.Q_Id} mentioned is not available";
                return View("NotFound");
            }

            EditQuestionViewModel model = new EditQuestionViewModel
            {
                Q_Id = question.Q_Id,
                Q_Text = question.Q_Text
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditQuestionViewModel model)
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            if (ModelState.IsValid)
            {
                var res = surveyRepository.GetQuestionById(model.Q_Id);

                if (res == null)
                {
                    ViewBag.Error = $"The id {res.Q_Id} mentioned is not available";
                    return View("NotFound");
                }

                res.Q_Text = model.Q_Text;
                surveyRepository.UpdatedQuestion(res);
                ViewBag.QuestionEdited = "The URL has been Successfully edited";
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            Question delQuestion = surveyRepository.GetQuestionById(id);
            surveyRepository.DeleteQuestion(delQuestion.Q_Id);
            return RedirectToAction("ListQuestion", "Admin");
        }

        [HttpGet]
        public IActionResult Result()
        {
            if (!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            return View(surveyRepository.GetAllResult());
        }
        [HttpGet]
        public IActionResult ListUser()
        {
            if(!Users.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index", "Survey");
            }
            return View(surveyRepository.GetUserList());
        }
    }
}
