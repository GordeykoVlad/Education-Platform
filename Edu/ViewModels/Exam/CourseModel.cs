using EducationPlatform.Models;
using Edu.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.ViewModels.Exam
{
    public class CourseModel : NavbarModel
    {
        public Course Course { get; set; }
        public Dictionary<Question, Answer[]> CourseData {get; set;}
        public int QuestionOrder { get; set; }
    }
}
