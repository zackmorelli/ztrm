using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection;


using ztrm.Models;
using ztrm.Models.RandomThoughts;
using ztrm.Services.Interfaces;

namespace ztrm.Services
{
    
    
    public class RandomThoughtsService : IRandomThoughtsService
    {

        private readonly ZTRMContext _ztrmContext;
        private readonly ILogger _logger;

        public RandomThoughtsService(ZTRMContext ztrmContext, ILogger<RandomThoughtsService> logger)
        {
            _ztrmContext = ztrmContext;
            _logger = logger;
        }


        public async Task<List<RandomThought>> GetAllRandomThoughtsAsync()
        {
            try
            {
                return await _ztrmContext.RandomThoughts.Include(rt => rt.RandomThoughtsCategories).ToListAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public async Task<List<RandomThoughtsCategoryLookup>> GetRandomThoughtsCategoryLookupsAsync()
        {
            try
            {
                return await _ztrmContext.RandomThoughtsCategoriesLookup.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public RandomThought GetRandomThought(int postId)
        {
            try
            {
                return _ztrmContext.RandomThoughts
                                   .Include(rt => rt.RandomThoughtsCategories)
                                     .ThenInclude(rtc => rtc.RandomThoughtsCategoryLookup)
                                   .FirstOrDefault(rt => rt.PostId == postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public string GetRandomThoughtText(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }



    }
}
