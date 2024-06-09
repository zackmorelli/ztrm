using Microsoft.EntityFrameworkCore;
using System.Reflection;


using ztrm.Models;
using ztrm.Models.RandomThoughts;
using ztrm.Services.Interfaces;

namespace ztrm.Services
{
    
    public class RandomThoughtsService : IRandomThoughtsService
    {

        private readonly IConfiguration _configuration;
        private readonly ZTRMContext _ztrmContext;
        private readonly ILogger _logger;

        public RandomThoughtsService(ZTRMContext ztrmContext, ILogger<RandomThoughtsService> logger, IConfiguration configuration)
        {
            _ztrmContext = ztrmContext;
            _logger = logger;
            _configuration = configuration;
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
                                   .FirstOrDefault(rt => rt.postid == postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        /// <summary>
        /// Given a filename, this determines the path to the random thought file based on the environment,
        /// retrieves it and returns the text as a string.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetRandomThoughtText(string fileName)
        {
            try
            {
                string randomThoughtsFileFullPath = _configuration.GetValue<string>("RandomThoughtsFilePath") + "\\" + fileName;
                return File.ReadAllText(randomThoughtsFileFullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in method {MethodName}", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }



    }
}
