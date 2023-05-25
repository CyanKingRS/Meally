using MeallyApp.Resources.ExceptionHandling;
using MeallyApp.Resources.Ingredients;
using MeallyApp.Resources.Services;
using MeallyApp.Resources.ViewIngredients;
using System.Collections.ObjectModel;

namespace MeallyApp.Resources.ViewModels
{
    public partial class UploadRecipeViewModel : BaseViewModel
    {
        IIngredientService ingredientService;

        IRecipeHandler recipeHandler;
        public ObservableCollection<Ingredient> IngredientsCollection { get; } = new();

        public ObservableCollection<Ingredients.Label> LabelsCollection { get; } = new();

        private IExceptionLogger logger;

        public Command GetIngredientsCommand { get; }

        public Command GetLabelsCommand { get; }

        public UploadRecipeViewModel(IIngredientService ingredientService, IRecipeHandler recipeHandler, IExceptionLogger logger)
        {
            Title = "Recipe Upload";
            this.ingredientService = ingredientService;
            this.recipeHandler = recipeHandler;
            this.logger = logger;
            GetIngredientsCommand = new Command(async () => await GetIngredientAsync());
            GetIngredientsCommand.Execute(this);
            GetLabelsCommand = new Command(async() => await GetLabelsAsync());
            GetLabelsCommand.Execute(this);
        }


        async Task GetIngredientAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var ingredients = await ingredientService.GetIngredients();

                if (IngredientsCollection.Count != 0)
                {
                    IngredientsCollection.Clear();
                }

                foreach (var ingredient in ingredients)
                {
                    IngredientsCollection.Add(ingredient);
                }
            }
            catch (Exception)
            {
                logger.WriteToLog("Unable to display products.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task GetLabelsAsync()
        {
            var labels = await recipeHandler.GetLabels();
            if (LabelsCollection.Count != 0)
            {
                LabelsCollection.Clear();
            }
            foreach (var label in labels)
            {
                LabelsCollection.Add(label);
            }
        }
    }
}
