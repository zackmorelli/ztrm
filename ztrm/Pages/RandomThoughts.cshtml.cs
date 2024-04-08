using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



using ztrm.Services;
using ztrm.Services.Interfaces;
using ztrm.Models.RandomThoughts;

namespace ztrm.Pages
{
    public class RandomThoughtsPageModel : PageModel
    {
        private readonly IRandomThoughtsService _randomThoughtsService;
        private readonly ILogger _logger;


        public List<RandomThought> RandomThoughts { get; set; }

        public List<RandomThoughtsCategoryLookup> RandomThoughtsCategories { get; set; }



        public RandomThoughtsPageModel(IRandomThoughtsService randomThoughtsService, ILogger<RandomThoughtsPageModel> logger)
        {
            _randomThoughtsService = randomThoughtsService;
            _logger = logger;

        }



        public void OnGet()
        {
            try
            {
                RandomThoughts = _randomThoughtsService.GetAllRandomThoughtsAsync().Result;
                RandomThoughtsCategories = _randomThoughtsService.GetRandomThoughtsCategoryLookupsAsync().Result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}
