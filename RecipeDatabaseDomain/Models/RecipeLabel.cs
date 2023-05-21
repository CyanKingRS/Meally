using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain.Models
{
    public class RecipeLabel
    {
        [Key]
        public int RecipeLabelId { get; set; }
        [Required]
        public int RecipeId { get; set; }

        [Required]
        public int LabelId { get; set; }
    }
}
