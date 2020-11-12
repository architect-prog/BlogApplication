﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
    }
}