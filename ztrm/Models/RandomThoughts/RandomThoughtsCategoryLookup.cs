using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ztrm.Models.RandomThoughts
{
    [Table("RandomThoughtsCategoriesLookup", Schema = "dbo")]
    public class RandomThoughtsCategoryLookup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        // Navigation property
        public List<RandomThoughtsCategory> RandomThoughtsCategories { get; set; }
    }
}
