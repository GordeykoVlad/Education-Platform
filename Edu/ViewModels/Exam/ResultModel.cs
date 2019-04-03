using EducationPlatform.Models;
using Edu.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.ViewModels.Exam
{
    public class ResultModel : NavbarModel
    {
        public Dictionary<Question, Answer> ResultData { get; set; }
        public Course Course { get; set; }
        public int Mark { get; set; }
    }
}
