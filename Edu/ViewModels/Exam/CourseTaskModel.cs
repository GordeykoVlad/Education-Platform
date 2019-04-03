using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models;
using Edu.ViewModels.Shared;

namespace EducationPlatform.ViewModels.Exam
{
    public class CourseTaskModel : NavbarModel
    {
        public CourseTask[] CourseTasks { get; set; }
        public CourseTask CourseTask { get; set; }
        public int CourseTaskOrder { get; set; }
        public string CourseName { get; set; }
        public SubHeadTask[] SubHeadTasks { get; set; }
    public SubHeadTask SubHeadTask { get; set; }
    public int SubHeadTaskOrder { get; set; }
    public string CourseTaskName { get; set; }
}
}
