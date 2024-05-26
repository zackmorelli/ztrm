using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ztrm.Models.RandomThoughts
{
    [Table("RandomThoughtsCategories", Schema = "dbo")]
    public class RandomThoughtsCategory
    {
        [Key, Column(Order = 0), ForeignKey("RandomThought")]
        public int PostId { get; set; }

        [Key, Column(Order = 1), ForeignKey("RandomThoughtsCategoryLookup")]
        public int CategoryId { get; set; }


        // Navigation properties
        public virtual RandomThought RandomThought { get; set; }
        public virtual RandomThoughtsCategoryLookup RandomThoughtsCategoryLookup { get; set; }
    }

}
