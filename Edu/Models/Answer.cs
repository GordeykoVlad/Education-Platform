﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int IdQuestion { get; set; }
        public bool IsCorrect { get; set; }
    }
}
