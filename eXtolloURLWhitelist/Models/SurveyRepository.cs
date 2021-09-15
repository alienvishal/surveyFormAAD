using eXtolloURLWhitelist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace eXtolloURLWhitelist.Models
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly SurveyDBContext surveyDBContext;

        public SurveyRepository(SurveyDBContext surveyDBContext)
        {
            this.surveyDBContext = surveyDBContext;
        }

        public Question DeleteQuestion(int id)
        {
            Question question = surveyDBContext.Questions.Find(id);
            if (question != null)
            {
                surveyDBContext.Questions.Remove(question);
                surveyDBContext.SaveChanges();
            }
            return question;
        }

        public List<ProjectInstance> GetAllProjectInstance()
        {
            return surveyDBContext.ProjectInstances.ToList();
        }

        public List<Question> GetAllQuestion()
        {
            return surveyDBContext.Questions.ToList();
        }

        public List<ListUserViewModel> GetAllResult()
        {
            var result = surveyDBContext.Questions.GroupJoin(surveyDBContext.Results,
                                                             q => q.Q_Id,
                                                             r => r.Q_Id,
                                                             (Question, Result) => new
                                                             {
                                                                 QText = Question.Q_Text,
                                                                 Answer = Result.Select(x => x.SelectedAnswer)
                                                             }).AsEnumerable();

            var model = new List<ListUserViewModel>();
            foreach (var question in result.AsEnumerable())
            {
                ListUserViewModel addListUserViewModel = new ListUserViewModel();
                addListUserViewModel.QText = question.QText;
                addListUserViewModel.SelectedAnswer = question.Answer.ToList();
                model.Add(addListUserViewModel);
            }
            return model;
            
        }

        public Question GetQuestionById(int id)
        {
            return surveyDBContext.Questions.Find(id);
        }

        public bool IsQuestionAdded(AddQuestionViewModel model)
        {
            Question question = new Question();
            question.Q_Text = model.UrlText;
            var res = surveyDBContext.Questions.Add(question);
            surveyDBContext.SaveChanges();

            if (res == null)
                return false;

            return true;
        }

        public bool IsSurveyAdded(SurveyViewModel model)
        {
            int flag = 0;
            for (int i = 0; i < model.Questions.Count; i++)
            {
                Result result = new Result
                {
                    User = model.UserId,
                    Project_Id = model.SelectedInstance,
                    Q_Id = model.Questions[i].Q_Id,
                    SelectedAnswer = model.SelectedAnswer[i]
                };
                var res = surveyDBContext.Results.Add(result);
                surveyDBContext.SaveChanges();

                if (res == null)
                    flag = 0;
                else
                    flag = 1;
            }

            if (flag == 0)
                return false;

            return true;
        }

        public bool IsSurveyTaken(SurveyViewModel model)
        {
            bool isSurveyTaken = false;
            var surveyTaken = surveyDBContext.Results
                                .Where(x => x.Project_Id == model.SelectedInstance)
                                .ToList();

            if (surveyTaken.Count > 0)
            {
                isSurveyTaken = true;
            }
            return isSurveyTaken;
        }

        public Question UpdatedQuestion(Question question)
        {
            var res = surveyDBContext.Questions.Attach(question);
            res.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            surveyDBContext.SaveChanges();
            return question;
        }
    }
}
