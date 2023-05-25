using RecipeDatabaseDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Ingredient>? Ingredients { get; set; }

        public List<Label> Labels { get; set; }

        public string? RecipeInstructions { get; set; }

        public string? Image { get; set; }

        public DateTime RecipeUploadDate { get; set; }

        public TimeSpan RecipeMakeTime { get; set; }

        public RecipeViewModel(Recipe recipe, List<Ingredient> ingredients, List<Label> labels)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Ingredients = ingredients;
            RecipeInstructions = recipe.RecipeInstructions;
            Image = recipe.Image;
            RecipeUploadDate = recipe.RecipeUploadDate;
            RecipeMakeTime = recipe.RecipeMakeTime;
            Labels = labels;
        }
    }
}
