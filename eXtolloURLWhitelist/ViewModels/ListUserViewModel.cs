﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.ViewModels
{
    public class ListUserViewModel
    {
        public string Email { get; set; }
        public string ProjectInstance { get; set; }
        public string Id { get; set; }
        public List<string> SelectedAnswer { get; set; }
        public string QText { get; set; }
    }
}
