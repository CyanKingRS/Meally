using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RecipeInstructions { get; set; }

        public string? Image { get; set; }

        public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }

        public ICollection<LabelRecipe> LabelRecipes { get; set; }

        [Required]
        public TimeSpan RecipeMakeTime { get; set; }

        [Required]
        public DateTime RecipeUploadDate { get; set; }

    }
}
