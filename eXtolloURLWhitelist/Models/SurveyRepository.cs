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
            var questionByResultJoin = (from qes in surveyDBContext.Questions
                          join res in surveyDBContext.Results
                          on qes.Q_Id equals res.Q_Id
                          select new
                          {
                              Question = qes.Q_Text,
                              Answer = res.SelectedAnswer
                          });

            var result = (from res in questionByResultJoin
                         group res by new
                         {
                             res.Question,
                             res.Answer
                         } into grp
                         orderby grp.Key.Question
                         select new
                         {
                             Q_Text = grp.Key.Question,
                             Votes = grp.Key.Answer
                         }).ToList();

            var model = new List<ListUserViewModel>();
            foreach (var question in result)
            {
                ListUserViewModel addListUserViewModel = new ListUserViewModel();
                addListUserViewModel.QText = question.Q_Text;
                addListUserViewModel.SelectedAnswer = question.Votes;
                
                model.Add(addListUserViewModel);
            }
            return model;
            
        }

        public Question GetQuestionById(int id)
        {
            return surveyDBContext.Questions.Find(id);
        }

        public List<ListSurveyUserViewModel> GetUserList()
        {
            var users = (from usr in surveyDBContext.Users
                         join project in surveyDBContext.ProjectInstances
                         on usr.ProjectId equals project.Project_Id
                         select new
                         {
                               Username = usr.UserName,
                               Project = project.ProjectName
                         }).ToList();
            var listuser = new List<ListSurveyUserViewModel>();
            foreach (var user in users)
            {
                ListSurveyUserViewModel model = new ListSurveyUserViewModel();
                model.UserId = user.Username;
                model.ProjectName = user.Project;

                listuser.Add(model);
            }
            return listuser;
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
            Users usr = new Users
            {
                UserName = model.UserId,
                ProjectId = model.SelectedInstance
            };
            var user = surveyDBContext.Users.Add(usr);
            surveyDBContext.SaveChanges();
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

                if (res == null || user == null)
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
