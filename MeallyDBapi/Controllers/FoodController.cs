using MeallyDBapi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using RecipeDatabaseDomain.Models;
using System.Drawing.Printing;

namespace MeallyDBapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        IMeallyDataRepository _meallyDataRepository;

        public FoodController(IMeallyDataRepository context)
        {
            _meallyDataRepository = context;
        }

        [HttpGet("GetIngredients")]
        public IActionResult GetIngredients()
        {
            return Ok(_meallyDataRepository.GetAllIngredients());
        }

        [HttpGet("GetRecipes")]
        public IActionResult GetRecipes()
        {
            return Ok(_meallyDataRepository.GetAllRecipes());
        }

        [HttpGet("GetRecipe/{id}")]
        public IActionResult GetRecipe(int id)
        {
            return Ok(_meallyDataRepository.GetRecipe(id));
        }

        [HttpGet("SearchRecipes/{text}")]
        public IActionResult SearchForRecipes(string text)
        {
            return Ok(_meallyDataRepository.SearchForRecipes(text));
        }

        [HttpGet("GetLabels")]
        public IActionResult GetLabels()
        {
            return Ok(_meallyDataRepository.GetLabels());
        }

        [HttpPost("UploadRecipe")]
        public IActionResult UploadRecipe([FromBody] NewRecipe request)
        {
            bool result = _meallyDataRepository.UploadRecipe(request);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
