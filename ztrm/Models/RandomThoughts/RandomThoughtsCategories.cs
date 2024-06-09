using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ztrm.Models.RandomThoughts
{
    [Table("randomthoughtscategories", Schema = "public")]
    public class RandomThoughtsCategory
    {
        [Key, Column(Order = 0), ForeignKey("RandomThought")]
        public int postid { get; set; }

        [Key, Column(Order = 1), ForeignKey("RandomThoughtsCategoryLookup")]
        public int categoryid { get; set; }


        // Navigation properties
        public virtual RandomThought RandomThought { get; set; }
        public virtual RandomThoughtsCategoryLookup RandomThoughtsCategoryLookup { get; set; }
    }

}
