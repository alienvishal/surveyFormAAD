using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public class Result
    {
        [Key]
        public int Res_Id { get; set; }
        [ForeignKey("ProjectInstance")]
        public int Project_Id { get; set; }
        public virtual ProjectInstance ProjectInstance { get; set; }
        [ForeignKey("Question")]
        public int Q_Id { get; set; }
        public virtual Question Question { get; set; }
        public string User { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
