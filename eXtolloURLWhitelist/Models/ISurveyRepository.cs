using eXtolloURLWhitelist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public interface ISurveyRepository
    {
        List<ProjectInstance> GetAllProjectInstance();
        List<Question> GetAllQuestion();
        Question GetQuestionById(int id);
        bool IsSurveyAdded(SurveyViewModel model);
        bool IsSurveyTaken(SurveyViewModel model);
        bool IsQuestionAdded(AddQuestionViewModel model);
        public Question UpdatedQuestion(Question question);
        public Question DeleteQuestion(int id);
        public List<ListUserViewModel> GetAllResult();
    }
}
