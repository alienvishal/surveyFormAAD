using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eXtolloURLWhitelist.Models
{
    public class SurveyDBContext : DbContext
    {
        public SurveyDBContext(DbContextOptions<SurveyDBContext> options):base(options)
        {

        }

        public DbSet<ProjectInstance> ProjectInstances { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
