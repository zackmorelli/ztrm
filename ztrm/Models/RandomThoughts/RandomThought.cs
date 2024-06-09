using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ztrm.Models.RandomThoughts
{
    [Table("randomthought", Schema = "public")]
    public class RandomThought
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int postid { get; set; }

        [Required]
        [MaxLength(255)]
        public string title { get; set; }

        [Required]
        public DateTime publisheddate { get; set; }

        [Required]
        [MaxLength(1000)]
        public string summary { get; set; }

        [Required]
        [MaxLength(255)]
        public string filename { get; set; }



        // Navigation property
        public List<RandomThoughtsCategory> RandomThoughtsCategories { get; set; }
    }
}
