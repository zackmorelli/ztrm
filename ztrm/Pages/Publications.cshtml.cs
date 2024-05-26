using System.Reflection;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ztrm.Pages
{
    public class PublicationsModel : PageModel
    {
        private readonly ILogger _logger;


        public PublicationsModel(ILogger<PublicationsModel> logger)
        {
            _logger = logger;
        }


        public void OnGet()
        {
            try
            {



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}
