using CommunityToolkit.Maui.Core.Extensions;
using MeallyApp.Resources.EventArguments;
using MeallyApp.Resources.Ingredients;
using MeallyApp.UserData;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace MeallyApp.Resources.Services
{
    public class RecipeHandler : IRecipeHandler
    {
        // This is a list of all recipes

        private List<Recipe> database = new List<Recipe>();

        private List<Ingredients.Label> labels = new();

        public IDatabaseConnection dbConnection;

        public RecipeHandler(IDatabaseConnection databaseConnection)
        {
            dbConnection = databaseConnection;
        }

        public async Task GetRecipesFromDB()
        {
            await dbConnection.GetDBAsync();
            database = dbConnection.GetRecipeList();
        }

        public async Task GetRecipesAPI()
        {
            var client = new HttpClient();

            string url = $"{User.BaseUrl}/api/food/getrecipes";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage respone = await client.GetAsync("");
            if (respone.IsSuccessStatusCode)
            {
                string content = respone.Content.ReadAsStringAsync().Result;
                database = JsonConvert.DeserializeObject<List<Recipe>>(content);
            }
        }

        // Set Compatibility rating on Recipe list
        public void SetComp(List<Ingredient> userIngredients)
        {
            if (database != null)
            {
                foreach (var recipe in database)
                {
                    var missingIngredients = recipe.Ingredients.Where(a => !User.inventory.Exists(b => b.DisplayName.Equals(a.DisplayName))).ToList();

                    double recipeCount = recipe.Ingredients.Count;
                    double missingCount = missingIngredients.Count;

                    recipe.Compatibility = Math.Round((recipeCount - missingCount) / recipeCount, 2);
                }
            }

        }

        // Order list of recipes by compatibility
        // LINQ to Objects usage (methods and queries)
        public void OrderDB()
        {
            if (database != null)
            {
                List<Recipe> tempList = database.OrderByDescending(x => x.Compatibility).ToList();
                for (int i = 0; i < database.Count; i++)
                {
                    database[i] = tempList[i];
                }
            }
        }

        // Print all recipes in database
        public void PrintDB()
        {
            foreach (var recipe in database)
            {
                Console.WriteLine($"[Name: {recipe.Name}] [Compatibility: {recipe.Compatibility}]\n");
                recipe.Ingredients.ForEach(i => Console.Write("{0}\t", i));
                Console.WriteLine("\n\n");
            }
        }

        public List<Recipe> GetRecipeList()
        {
            return new List<Recipe>(database);
        }

        public async Task SearchForRecipes(string Text)
        {
            var client = new HttpClient();
            string url = $"{User.BaseUrl}/api/food/searchrecipes/{Text}";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage respone = await client.GetAsync("");
            if (respone.IsSuccessStatusCode)
            {
                string content = respone.Content.ReadAsStringAsync().Result;
                database = JsonConvert.DeserializeObject<List<Recipe>>(content);
            }
        }

        public async Task<List<Ingredients.Label>> GetLabels()
        {
            var client = new HttpClient();
            string url = $"{User.BaseUrl}/api/food/getlabels";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                labels = JsonConvert.DeserializeObject<List<Ingredients.Label>>(content);
                return labels;
            }
            else
            { return labels; }
        }
        public async Task FilterRecipes(Ingredients.Label label)
        {
            var client = new HttpClient();

            string url = $"{User.BaseUrl}/api/food/getrecipes";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage respone = await client.GetAsync("");
            if (respone.IsSuccessStatusCode)
            {
                string content = respone.Content.ReadAsStringAsync().Result;
                database = JsonConvert.DeserializeObject<List<Recipe>>(content);
                List<Recipe> tmp = new ();
                foreach (var r in database)
                {
                    foreach (var l in r.Labels) 
                    {
                        if (l.Id == label.Id)
                        {
                            tmp.Add(r);
                        }
                    }
                }
                database = tmp;
            }
         
        }

    }  

}
