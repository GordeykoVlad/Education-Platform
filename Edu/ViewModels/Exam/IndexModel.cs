using EducationPlatform.Models;
using Edu.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.ViewModels.Exam
{
    public class IndexModel : NavbarModel
    {
        public Course[] Courses { get; set; }
        public Course Course { get; set; }
        public int CourseOrder { get; set; }
        public string LanguageName { get; set; }
    }
}
