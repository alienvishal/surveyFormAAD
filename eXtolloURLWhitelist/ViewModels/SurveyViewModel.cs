using eXtolloURLWhitelist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.ViewModels
{
    public class SurveyViewModel
    {
        public List<Question> Questions { get; set; }
        public string[] RadioButton = new[] { "Keep", "Discard" };
        public string UserId { get; set; }
        public int SelectedInstance { get; set; }
        public string[] SelectedAnswer { get; set; }
    }
}
