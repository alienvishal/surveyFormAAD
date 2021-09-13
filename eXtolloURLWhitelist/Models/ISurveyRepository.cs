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
    }
}
