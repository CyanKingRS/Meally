using Flurl.Http;
using MeallyApp.Resources.Ingredients;
using MeallyApp.Resources.Services;
using MeallyApp.Resources.ViewIngredients;
using MeallyApp.Resources.ViewModels;
using MeallyApp.UserData;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MeallyApp;

public partial class UploadRecipePage : ContentPage
{

	public IRecipeHandler recipeHandler;

    private List<Ingredient> selectionIngredients = new List<Ingredient>();

	private List<Resources.Ingredients.Label> selectionLabels = new List<Resources.Ingredients.Label>();

    public UploadRecipePage(UploadRecipeViewModel viewModel, IRecipeHandler recipeHandler)
	{
		InitializeComponent();
		BindingContext = viewModel;
		this.recipeHandler = recipeHandler;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		selectionIngredients.Clear();
		selectionLabels.Clear();

        if (IngridientView.SelectedItems != null && LabelView.SelectedItems != null)
        {
            // CollectionView returns IList<object>, code below casts IList<object> to List<Ingredients>
            object tempObject;

            foreach (var o in IngridientView.SelectedItems)
            {
                tempObject = o;
                // Widening and narrowing type conversions
                selectionIngredients.Add(tempObject as Ingredient);
            }

            foreach (var o in LabelView.SelectedItems)
            {
                tempObject = o;
                selectionLabels.Add(tempObject as Resources.Ingredients.Label);
            }

            IngridientView.SelectedItems.Clear();
            LabelView.SelectedItems.Clear();

            // Add request for sending inventory to API
            try
            {
                var requestBody = new
                {
                    Username = User.UserName,
                    RecipeName = RecipeName.Text,
                    RecipeDescription = RecipeDescription.Text,
                    RecipeCreationTime = DateTime.Now,
                    RecipeMakeTime = TimePicker.Time,
                    Ingredients = new List<Ingredient>(selectionIngredients),
                    Labels = new List<Resources.Ingredients.Label>(selectionLabels)
                };
                Debug.WriteLine(JsonConvert.SerializeObject(requestBody));
                var result = await $"{User.BaseUrl}/api/food/uploadrecipe".PostJsonAsync(new
                {
                    Username = User.UserName,
                    RecipeName = RecipeName.Text,
                    RecipeDescription = RecipeDescription.Text,
                    RecipeCreationTime = DateTime.Now,
                    RecipeMakeTime = TimePicker.Time,
                    Ingredients = new List<Ingredient>(selectionIngredients),
                    Labels = new List<Resources.Ingredients.Label>(selectionLabels)
                }); 
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            //result RETURNS StatusCode.200K if everything ok and StatusCode.404 if somtehing went wrong.
        }
    }
}