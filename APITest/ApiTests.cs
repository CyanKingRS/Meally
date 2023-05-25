using MeallyAPI;
using MeallyDBapi.Controllers;
using MeallyDBapi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory;
using Newtonsoft.Json;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using RecipeDatabaseDomain;
using RecipeDatabaseDomain.Models;
using RecipeDatabaseDomain.ViewModels;
using System.Diagnostics;

namespace APITest
{
    [TestClass]
    public class ApiTests
    {
        private WebApplicationFactory<Program> webAppFactory;
        private HttpClient httpClient;
        private DbContextOptions<RecipeContext> _contextOptions;
        public ApiTests()
        {
            webAppFactory = new WebApplicationFactory<Program>();
            httpClient = webAppFactory.CreateDefaultClient();
            _contextOptions = new DbContextOptionsBuilder<RecipeContext>().UseInMemoryDatabase("GetIngredientsTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
       
        [TestMethod]
        public async Task GetRecipes_ReturnsIngredients()
        { 
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var repository = new MeallyDataRepository(context);

            context.Ingredients.AddRange(
                new Ingredient { DisplayName = "Cheese", Id = 1 },
                new Ingredient { DisplayName = "Ham", Id = 2 }
                );

            var foodController = new FoodController(repository);

            var response = foodController.GetIngredients() as OkObjectResult;

            List<Ingredient>? ingredients = response.Value as List<Ingredient>;

            Assert.IsNotNull(ingredients);

            for (int i = 0; i < ingredients.Count; i++)
            {
                Assert.AreEqual(i + 1 , ingredients[i].Id);
            }
        }

        [TestMethod]
        public async Task GetIngredients_ReturnsRecipes()
        {
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var repository = new MeallyDataRepository(context);

            context.Ingredients.AddRange(
                new Ingredient { DisplayName = "Cheese", Id = 1 },
                new Ingredient { DisplayName = "Ham", Id = 2 }
                );

            context.Labels.AddRange(
                new RecipeDatabaseDomain.Models.Label { Name = "Dairy", Id = 1 },
                new RecipeDatabaseDomain.Models.Label { Name = "Meat", Id = 2 }
                );

            List<LabelRecipe> labelRecipes = new List<LabelRecipe> 
            {
                new LabelRecipe {Id=1, LabelID=1, RecipeID=1},
                new LabelRecipe {Id=2, LabelID=2, RecipeID=1}
            };

            List<RecipeIngredient> recipeIngredients= new List<RecipeIngredient>
            {
                new RecipeIngredient {Id=1, IngredientID=1, RecipeID=1},
                new RecipeIngredient {Id=2, IngredientID=2, RecipeID=1}
            };

            context.Recipes.Add(
                new Recipe { Id = 1, Name="Cheese And Ham", RecipeInstructions="Melt cheese over ham.", RecipeMakeTime=new TimeSpan(2,2,2), RecipeUploadDate=new DateTime(2023,01,01), LabelRecipes=labelRecipes, RecipeIngredients=recipeIngredients}
                );

            context.SaveChanges();

            var foodController = new FoodController(repository);

            var response = foodController.GetRecipes() as OkObjectResult;

            //Debug.Print(response.Value.GetType().Name);

            List<RecipeViewModel>? recipes = response.Value as List<RecipeViewModel>;

            Assert.IsNotNull(recipes);
            Assert.AreEqual(1, recipes.Count);

            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(i + 1, recipes[i].Id); 
            }
        }

        [TestMethod]
        public async Task GetIngredients_ReturnsRecipeWithId()
        {
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var repository = new MeallyDataRepository(context);

            context.Ingredients.AddRange(
                new Ingredient { DisplayName = "Cheese", Id = 1 },
                new Ingredient { DisplayName = "Ham", Id = 2 }
                );

            context.Labels.AddRange(
                new RecipeDatabaseDomain.Models.Label { Name = "Dairy", Id = 1 },
                new RecipeDatabaseDomain.Models.Label { Name = "Meat", Id = 2 }
                );

            List<LabelRecipe> labelRecipes = new List<LabelRecipe>
            {
                new LabelRecipe {Id=1, LabelID=1, RecipeID=1},
                new LabelRecipe {Id=2, LabelID=2, RecipeID=1}
            };

            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>
            {
                new RecipeIngredient {Id=1, IngredientID=1, RecipeID=1},
                new RecipeIngredient {Id=2, IngredientID=2, RecipeID=1}
            };

            context.Recipes.Add(
                new Recipe { Id = 1, Name = "Cheese And Ham", RecipeInstructions = "Melt cheese over ham.", RecipeMakeTime = new TimeSpan(2, 2, 2), RecipeUploadDate = new DateTime(2023, 01, 01), LabelRecipes = labelRecipes, RecipeIngredients = recipeIngredients }
                );

            context.SaveChanges();

            var foodController = new FoodController(repository);

            var response = foodController.GetRecipe(1) as OkObjectResult;

            //Debug.Print(response.Value.GetType().Name);

            Recipe? recipe = response.Value as Recipe;
            

            Assert.IsNotNull(recipe);
            Assert.AreEqual(1, recipe.Id);
            Assert.AreEqual("Cheese And Ham", recipe.Name);
        }

        [TestMethod]
        public async Task GetIngredients_ReturnsUserInventory()
        {
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            context.Ingredients.AddRange(
                new Ingredient { DisplayName = "Cheese", Id = 1 },
                new Ingredient { DisplayName = "Ham", Id = 2 }
                );

            //context.UserAccounts.Add(new UserAccount { FirstName = "Test", LastName = "Test", UserID = 1, UserName = "Test", Password = "Test", ConfirmPassword = "Test" });

            

            var repository = new MeallyDataRepository(context);

            var user = new UserAccount
            {
                FirstName = "Test",
                LastName = "Test",
                UserID = 1,
                UserName = "Test",
                Password = "Test",
                ConfirmPassword = "Test"
            };

            context.UserIngredients.AddRange(
                new UserIngredient { Id = 1, IngredientId = 1, UserId = 1 },
                new UserIngredient { Id = 2, IngredientId = 2, UserId = 1 }
                );

            repository.RegisterUser(user);

            var userController = new UserController(repository);

            var response = userController.VerifyUser("Test", "Test");

            Debug.Print(response.GetType().Name);

            List<Ingredient>? inventory = (response as OkObjectResult).Value as List<Ingredient>;

            Assert.IsNotNull(inventory);
            Assert.AreEqual(2, inventory.Count);
            Assert.AreEqual("Cheese", inventory[0].DisplayName);
            Assert.AreEqual("Ham", inventory[1].DisplayName);
        }

        [TestMethod]
        public async Task GetLabelsTest()
        {
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Labels.AddRange(
                new Label { Name = "Cheesy", Id = 1 },
                new Label { Name = "Hamy", Id = 2 }
            );

            context.SaveChanges();

            var repository = new MeallyDataRepository(context);

            var foodController = new FoodController(repository);

            var response = foodController.GetLabels() as OkObjectResult;

            Debug.Print(response.Value.GetType().Name) ;

            var labels = response.Value as List<RecipeDatabaseDomain.Models.Label>;

            Assert.IsNotNull(labels);

            Assert.AreEqual(2, labels.Count);

            for(int i = 0; i < labels.Count; ++i)
            {
                Assert.AreEqual(i + 1, labels[i].Id);
            }

            Assert.AreEqual("Cheesy", labels[0].Name);
            Assert.AreEqual("Hamy", labels[1].Name);
        }



        [TestMethod]
        public async Task UploadRecipeTest()
        {
            using var context = new RecipeContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Ingredients.AddRange(
                new Ingredient { DisplayName = "Cheese", Id = 1 },
                new Ingredient { DisplayName = "Ham", Id = 2 }
                );

            context.Labels.AddRange(
                new RecipeDatabaseDomain.Models.Label { Name = "Dairy", Id = 1 },
                new RecipeDatabaseDomain.Models.Label { Name = "Meat", Id = 2 }
                );

            context.SaveChanges();

            List<RecipeDatabaseDomain.Models.Label> labels = new List<RecipeDatabaseDomain.Models.Label>
            {
                new RecipeDatabaseDomain.Models.Label {Id=1, Name="Dairy"},
                new RecipeDatabaseDomain.Models.Label {Id=2, Name="Meat"}
            };

            List<Ingredient> ingredients = new List<Ingredient>
            {
                new Ingredient {Id=1, DisplayName="Cheese"},
                new Ingredient {Id=2, DisplayName="Ham"}
            };

            var requestBody = new RecipeDatabaseDomain.Models.NewRecipe
            {
                Username = "Test",
                RecipeName = "Test",
                RecipeDescription = "Test",
                RecipeCreationTime = new DateTime(2023, 1, 1),
                RecipeMakeTime = new TimeSpan(2,2,2),
                Ingredients = new List<Ingredient>(ingredients),
                Labels = new List<RecipeDatabaseDomain.Models.Label>(labels)
            };

            var user = new UserAccount
            {
                FirstName = "Test",
                LastName = "Test",
                UserID = 1,
                UserName = "Test",
                Password = "Test",
                ConfirmPassword = "Test"
            };

            

            var repository = new MeallyDataRepository(context);

            repository.RegisterUser(user);

            var foodController = new FoodController(repository);

            var response = foodController.UploadRecipe(requestBody) as OkObjectResult;

            var recipeResponse = foodController.GetRecipe(1) as OkObjectResult;

            var gotRecipe = recipeResponse.Value as Recipe;

            Assert.IsNotNull(gotRecipe);

            Assert.AreEqual(gotRecipe.Id, 1);

            Assert.AreEqual(gotRecipe.Name, "Test");
        }

        
    }


}