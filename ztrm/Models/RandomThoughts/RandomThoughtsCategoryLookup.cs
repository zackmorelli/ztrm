using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ztrm.Models.RandomThoughts
{
    [Table("randomthoughtscategorieslookup", Schema = "public")]
    public class RandomThoughtsCategoryLookup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int categoryid { get; set; }

        [Required]
        public string categoryname { get; set; }

        // Navigation property
        public List<RandomThoughtsCategory> RandomThoughtsCategories { get; set; }
    }
}
