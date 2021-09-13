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
    }
}
