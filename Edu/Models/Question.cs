﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int IdCourse { get; set; }
    }
}
