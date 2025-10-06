using System.ComponentModel.DataAnnotations;

namespace ztrm.Models
{
    public class NasaOptions
    {
        [Required] public string ApiKey { get; set; } = "";
    }
}
