using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ztrm.Models.RandomThoughts
{
    public class RandomThought
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Summary{ get; set; }

        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; }



        // Navigation property
        public List<RandomThoughtsCategory> RandomThoughtsCategories { get; set; }
    }
}
