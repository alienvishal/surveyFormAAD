using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        [ForeignKey("ProjectInstance")]
        public int ProjectId { get; set; }
        public virtual ProjectInstance ProjectInstance { get; set; }
    }
}
