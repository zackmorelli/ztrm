using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;


namespace ztrm.Pages
{
    public class AboutMeModel : PageModel
    {
        private readonly ILogger _logger;


        public AboutMeModel(ILogger<AboutMeModel> logger)
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
