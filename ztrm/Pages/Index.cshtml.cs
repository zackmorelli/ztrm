using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Reflection;
using ztrm.Models.DTOs;
using ztrm.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ztrm.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INasaService _nasa;

        public IndexModel(ILogger<IndexModel> logger, INasaService nasa)
        {
            _logger = logger;
            _nasa = nasa;
        }

        public ApodDto? Apod { get; private set; }
        public string? Error { get; private set; }

        public async Task OnGetAsync(CancellationToken ct = default)
        {
            try
            {
                Apod = await _nasa.GetApodAsync(null, hd: true, thumbs: true, ct);
                if (Apod is null) Error = "Could not load NASA APOD right now.";
            }
            catch (Exception ex)
            {
                Error = "Could not load NASA APOD right now.";
            }
        }
    }
}
