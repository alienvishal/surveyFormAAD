using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public class ProjectInstance
    {
        [Key]
        public int Project_Id { get; set; }
        public string ProjectName { get; set; }
    }
}
