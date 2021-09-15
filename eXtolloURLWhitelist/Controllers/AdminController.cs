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
    [Authorize("admin-only")]
    public class AdminController:Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly ISurveyRepository surveyRepository;

        public AdminController(ILogger<AdminController> _logger, ISurveyRepository surveyRepository)
        {
            logger = _logger;
            this.surveyRepository = surveyRepository;
        }
        [HttpGet]
        public IActionResult AddQuestion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuestion(AddQuestionViewModel model)
        {
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
            return View(surveyRepository.GetAllQuestion());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
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
            Question delQuestion = surveyRepository.GetQuestionById(id);
            surveyRepository.DeleteQuestion(delQuestion.Q_Id);
            return RedirectToAction("ListQuestion", "Admin");
        }

        [HttpGet]
        public IActionResult Result()
        {
            return View(surveyRepository.GetAllResult());
        }
    }
}
