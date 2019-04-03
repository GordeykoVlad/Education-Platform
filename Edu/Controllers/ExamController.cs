using EducationPlatform.Repositories;
using EducationPlatform.ViewModels.Exam;
using Microsoft.AspNetCore.Mvc;
using Edu.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Controllers
{
    public class ExamController : Controller
    {
        private readonly AnswerRepository _answerRepository;
        private readonly CourseRepository _courseRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly UserRepository _userRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly CourseTaskRepository _courseTaskRepository;
        private readonly SubHeadTaskRepository _subHeadTaskRepository;

        public ExamController()
        {
            _courseRepository = new CourseRepository();
            _answerRepository = new AnswerRepository();
            _questionRepository = new QuestionRepository();
            _userRepository = new UserRepository();
            _languageRepository = new LanguageRepository();
            _courseTaskRepository = new CourseTaskRepository();
            _subHeadTaskRepository = new SubHeadTaskRepository();
        }
        // GET
        public async Task<IActionResult> Index(int languageId, string languagename)
        {
            //return View();
            var model = new IndexModel();
           
           // var course = await _courseRepository.Get(courseId);
            var courses = await _courseRepository.GetByLanguage(languageId);
            //model.Course = courses[courseOrder];
            // model.CourseOrder = courseOrder;
            model.Courses = courses;
            model.LanguageName = languagename;
            //model.Language.Name = languagename;





            if (!User.Identity.IsAuthenticated)
                return View(model);

            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;

          
         

            return View(model);
        }
        public async Task<IActionResult> Course(int courseId)
        {
           
            //return View();
            var model = new CourseModel();
            model.CourseData = new Dictionary<Models.Question, Models.Answer[]>();
            //курсы
            model.Course = await _courseRepository.Get(courseId);
            var questions = await _questionRepository.GetByCourse(courseId);
            foreach(var question in questions)
            {
                var answers = await _answerRepository.GetByQuestion(question.Id);
                model.CourseData.Add(question, answers);
            }
           

            //model.QuestionOrder = questionOrder;
            //ответ

            if (!User.Identity.IsAuthenticated)
                return View(model);
            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;

            return View(model);
        }

        public async Task<IActionResult> Result(int courseId)
        {
            var model = new ResultModel();
            model.Course = await _courseRepository.Get(courseId);


            model.ResultData = new Dictionary<Models.Question, Models.Answer>();
            foreach (var record in Request.Form)
            {
                var questionId = Convert.ToInt32(record.Key);
                var answerId = Convert.ToInt32(record.Value[0]);
                var question = await _questionRepository.Get(questionId);
                var answer = await _answerRepository.Get(answerId);
                model.ResultData.Add(question, answer);
                if (answer.IsCorrect)
                {
                    model.Mark++;
                }
            }

            if (!User.Identity.IsAuthenticated)
                return View(model);
            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;

            return View(model);
        }


        public async Task<IActionResult> Language()
        {
            var model = new LanguageModel();
            var languages = await _languageRepository.GetAll();
            model.Languages = languages;

            if (!User.Identity.IsAuthenticated)
                return View(model);
            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;

            return View(model);
        }


        public async Task<IActionResult> CourseTask()
        {
            var model = new CourseTaskModel();
            var coursetasks = await _courseTaskRepository.GetAll();
            model.CourseTasks = coursetasks;
            var subheadtasks = await _subHeadTaskRepository.GetAll();
            model.SubHeadTasks = subheadtasks;

            if (!User.Identity.IsAuthenticated)
                return View(model);
            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;

            return View(model);
        }




    }
}
