using MeallyAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using RecipeDatabaseDomain.Models;
using Newtonsoft.Json;
using RecipeDatabaseDomain.ViewModels;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Diagnostics;

namespace APITest
{
    [TestClass]
    public class ApiTests
    {
        private WebApplicationFactory<Program> webAppFactory;
        private HttpClient httpClient;
        public ApiTests()
        {
            webAppFactory = new WebApplicationFactory<Program>();
            httpClient = webAppFactory.CreateDefaultClient();
        }
       
        [TestMethod]
        public async Task GetRecipes_ReturnsIngredients()
        {
            var response = await httpClient.GetAsync("api/food/getingredients");
            var stringResults = await response.Content.ReadAsStringAsync();

            List<Ingredient>? ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(stringResults);

            Assert.IsNotNull(ingredients);

            for (int i = 0; i < ingredients.Count; i++)
            {
                Assert.AreEqual(i + 1 , ingredients[i].Id);
            }
        }
        [TestMethod]
        public async Task GetIngredients_ReturnsRecipes()
        {
            var response = await httpClient.GetAsync("api/food/getrecipes");
            var stringResults = await response.Content.ReadAsStringAsync();

            List<Recipe>? recipes = JsonConvert.DeserializeObject<List<Recipe>>(stringResults);

            Assert.IsNotNull(recipes);
            Assert.AreEqual(2, recipes.Count);

            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(i + 1, recipes[i].Id); 
            }
        }

        [TestMethod]
        public async Task GetIngredients_ReturnsRecipeWithId()
        {
            var response = await httpClient.GetAsync("api/food/getrecipe/1");
            Assert.IsNotNull(response);
            var stringResults = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(stringResults);
            Recipe? recipe = JsonConvert.DeserializeObject<Recipe>(stringResults);

            Assert.IsNotNull(recipe);
            Assert.AreEqual(1, recipe.Id);
            Assert.AreEqual("Test", recipe.Name);
        }

        [TestMethod]
        public async Task GetIngredients_ReturnsUserInventory()
        {
            var values = new Dictionary<string, string>
            {
                { "username", "Test" },
                { "password", "Test" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await httpClient.PostAsync("api/user/verifyuser?username=Test&password=Test", content);
            var stringResults = await response.Content.ReadAsStringAsync();

            List<Ingredient>? inventory = JsonConvert.DeserializeObject<List<Ingredient>>(stringResults);

            Assert.IsNotNull(inventory);
            Assert.AreEqual(0, inventory.Count);
            //Assert.AreEqual("Carrot", inventory[0].DisplayName);
            //Assert.AreEqual("Celery", inventory[1].DisplayName);
        }
    }
}