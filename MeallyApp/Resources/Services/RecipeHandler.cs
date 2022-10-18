﻿using MeallyApp.Resources.Ingredients;
using MeallyApp.UserData;
using Newtonsoft.Json;

namespace MeallyApp.Resources.Services
{
    public static class RecipeHandler
    {
        // This is a list of all recipes
        // Generic
        public static List<Recipe> database = new List<Recipe>();

        // This is path to database file
        public static string DBPath = null;


        /* Get database of recipes from .json file
        5. Reading from file */
        public static void GetDB()
        {
            var inputStream = new FileStream(DBPath, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(inputStream);
            string jsonData = streamReader.ReadToEnd();
            database = JsonConvert.DeserializeObject<List<Recipe>>(jsonData);
        }

        // Set Compatibility rating on Recipe list
        public static void SetComp(List<Ingredient> userIngredients)
        {
            foreach (var recipe in database)
            {
                var missingIngredients = recipe.Ingredients.Where(a => !User.inventory.Exists(b => b.ingredient.Equals(a.ingredient))).ToList();

                double recipeCount = recipe.Ingredients.Count;
                double missingCount = missingIngredients.Count;

                recipe.Compatibility = Math.Round((recipeCount - missingCount) / recipeCount,2);
            }
        }

        // Order list of recipes by compatibility
        // LINQ to Objects usage (methods and queries)
        public static void OrderDB()
        {
            database = database.OrderByDescending(x => x.Compatibility).ToList();
        }

        // Print all recipes in database
        public static void PrintDB()
        {
            foreach (var recipe in database)
            {
                Console.WriteLine($"[Name: {recipe.Name}] [Compatibility: {recipe.Compatibility}]\n");
                recipe.Ingredients.ForEach(i => Console.Write("{0}\t", i));
                Console.WriteLine("\n\n");
            }
        }

        // 4. Extension method usage for recipe
        public static void PrintRecipe(this Recipe recipe)
        {
            Console.WriteLine($"[Name: {recipe.Name}] [Compatibility: {recipe.Compatibility}]\n");
            recipe.Ingredients.ForEach(i => Console.Write("{0}\t", i));
            Console.WriteLine("\n\n");
        }

        public static async Task<List<Recipe>> GetRecipeList()
        {
            return new List<Recipe>(database);
        }

    }

}