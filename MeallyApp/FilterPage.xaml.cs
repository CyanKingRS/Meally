using MeallyApp.UserData;
using MeallyApp.Resources.Services;
using MeallyApp.Resources.ViewIngredients;
using MeallyApp.Resources.ExceptionHandling;
using MeallyApp.Resources.EventArguments;
using MeallyApp.Resources.Ingredients;

namespace MeallyApp;

public delegate void ExceptionDelegate();

public partial class FilterPage : ContentPage
{
    public RecipeViewModel ViewModel { get; }

    // 4. Delegate usage

    private ExceptionDelegate action;
    private bool isPageCreated = false;
    private IExceptionLogger logger;
    private IDatabaseConnection databaseConnection;
    private IRecipeHandler recipeHandler;

    public FilterPage(RecipeViewModel viewModel, IExceptionLogger logger, IDatabaseConnection connection, IRecipeHandler recipeHandler)
    {
        InitializeComponent();
        BindingContext = viewModel;
        ViewModel = viewModel;

        this.logger = logger;
        this.databaseConnection = connection;
        this.recipeHandler = recipeHandler;

        //Subscribing to custom event
        logger.exceptionAddedToFile += delegate (string message)  // added event handler method to a custom event
        {
            System.Diagnostics.Debug.WriteLine(message);
        };

        //Subscribing to standart event
        databaseConnection.RecipesLoaded += delegate (object sender, RecipesLoadedEventArgs arguments)  // added even handler method to a standart event
        {
            System.Diagnostics.Debug.WriteLine("Standart event:  " + arguments.message);
        };
        action = logger.ClearLog;
        action();
    }

    private void ExceptionLogButton_OnClicked(object sender, EventArgs e)
    {
        action = logger.ReadFromLog;
        action();
    }

    private async void RecipeButton_OnClicked(object sender, EventArgs e)
    {
        if (!Loader.IsRunning)
        {
            Loader.IsRunning = true;
            await recipeHandler.GetRecipesAPI();
            recipeHandler.SetComp(User.inventory);
            recipeHandler.OrderDB();
            Loader.IsRunning = false;
            ViewModel.GetRecipesCommand.Execute(this);
        }
    }

    private async void Load_Recipes()
    {
        if (!Loader.IsRunning)
        {
            Loader.IsRunning = true;
            await recipeHandler.GetRecipesAPI();
            recipeHandler.SetComp(User.inventory);
            recipeHandler.OrderDB();
            Loader.IsRunning = false;
            ViewModel.GetRecipesCommand.Execute(this);
        }
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isPageCreated)
        {
            Load_Recipes();
            isPageCreated= true;
        }
        
    }

    private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        if (!Loader.IsRunning)
        {
            Loader.IsRunning = true;
            SearchBar searchBar = (SearchBar)sender;
            await recipeHandler.SearchForRecipes(searchBar.Text);
            recipeHandler.SetComp(User.inventory);
            recipeHandler.OrderDB();
            Loader.IsRunning = false;
            ViewModel.GetRecipesCommand.Execute(this);
        }

    }

    private async void LabelView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!Loader.IsRunning)
        {
            Loader.IsRunning = true;
            CollectionView labelCollection = (CollectionView)sender;
            if (labelCollection.SelectedItem != null)
            {
                await recipeHandler.FilterRecipes(labelCollection.SelectedItem as MeallyApp.Resources.Ingredients.Label);
            }
            else
            {
                await recipeHandler.GetRecipesAPI();
            }
            recipeHandler.SetComp(User.inventory);
            recipeHandler.OrderDB();
            Loader.IsRunning = false;
            ViewModel.GetRecipesCommand.Execute(this);
        }
    }
}