using CommunityToolkit.Mvvm.Input;
using MeallyApp.Resources.ExceptionHandling;
using MeallyApp.Resources.Ingredients;
using MeallyApp.Resources.Services;
using System.Collections.ObjectModel;

namespace MeallyApp.Resources.ViewIngredients
{
    public partial class RecipeViewModel : BaseViewModel
    {
        public ObservableCollection<Recipe> Recipes { get; } = new();

        public ObservableCollection<Ingredients.Label> Labels { get; } = new();

        private IExceptionLogger logger;
        private IRecipeHandler recipeHandler;

        public Command GetRecipesCommand { get; }
        public Command GetLabelsCommand { get;  }


        public RecipeViewModel(IExceptionLogger logger, IRecipeHandler recipeHandler)
        {
            Title = "Recipes";
            this.logger = logger;
            this.recipeHandler = recipeHandler;
            GetRecipesCommand = new Command(async () => await GetRecipesAsync());
            GetLabelsCommand = new Command(async () => await GetLabelsAsync());
            GetLabelsCommand.Execute(this);
            GetRecipesCommand.Execute(this);
        }

        async Task GetRecipesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var recipes = recipeHandler.GetRecipeList();

                if (Recipes.Count != 0)
                {
                    Recipes.Clear();
                }

                foreach (var recipe in recipes)
                {
                    Recipes.Add(recipe);
                }
            }
            catch (Exception)
            {
                logger.WriteToLog("Unable to display recipes.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task GetLabelsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var labels = await recipeHandler.GetLabels();

                if (Recipes.Count != 0)
                {
                    Recipes.Clear();
                }

                foreach (var label in labels)
                {
                    Labels.Add(label);
                }
            }
            catch (Exception)
            {
                logger.WriteToLog("Unable to display labels.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task Tap(Recipe recipe)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}",true , new Dictionary<string, object> { ["Recipe"] = recipe });
        }

    }
}
