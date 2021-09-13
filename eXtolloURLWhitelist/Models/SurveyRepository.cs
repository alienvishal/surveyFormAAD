using eXtolloURLWhitelist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly SurveyDBContext surveyDBContext;

        public SurveyRepository(SurveyDBContext surveyDBContext)
        {
            this.surveyDBContext = surveyDBContext;
        }
        public List<ProjectInstance> GetAllProjectInstance()
        {
            return surveyDBContext.ProjectInstances.ToList();
        }

        public List<Question> GetAllQuestion()
        {
            return surveyDBContext.Questions.ToList();
        }

        public Question GetQuestionById(int id)
        {
            return surveyDBContext.Questions.Find(id);
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
    }
}
