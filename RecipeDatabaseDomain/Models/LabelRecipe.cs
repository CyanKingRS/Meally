﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain.Models
{
    public class LabelRecipe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RecipeID { get; set; }
        [Required]
        public int LabelID { get; set; }

    }
}