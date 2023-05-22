using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain.Models
{
    public class NewRecipe
    {
        public string Username { get; set; }

        public string RecipeName { get; set; }

        public string RecipeDescription { get; set; }

        public DateTime RecipeCreationTime { get; set; }

        public TimeSpan RecipeMakeTime { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public List<Label> Labels { get; set; }
    }
}
