using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;


using ztrm.Services;
using ztrm.Services.Interfaces;
using ztrm.Models.RandomThoughts;


namespace ztrm.Pages
{
    public class RandomThoughtPageModel : PageModel
    {
        private readonly IRandomThoughtsService _randomThoughtsService;
        private readonly ILogger _logger;


        public RandomThought RandomThought { get; set; }

        public string RandomThoughtText { get; set; }

        public RandomThoughtPageModel(IRandomThoughtsService randomThoughtsService, ILogger<RandomThoughtsPageModel> logger)
        {
            _randomThoughtsService = randomThoughtsService;
            _logger = logger;

        }


        public IActionResult OnGet(int? postId)
        {
            try
            {
                if (postId.HasValue)
                {
                    RandomThought = _randomThoughtsService.GetRandomThought(postId.Value);

                    if (RandomThought != null && RandomThought != default)
                    {
                        //retrieve actual text of random thought post
                        RandomThoughtText = _randomThoughtsService.GetRandomThoughtText(RandomThought.FileName);

                        return Page();
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }












    }
}
