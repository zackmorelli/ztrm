using ztrm.Models.RandomThoughts;

namespace ztrm.Services.Interfaces
{
    public interface IRandomThoughtsService
    {

        Task<List<RandomThought>> GetAllRandomThoughtsAsync();

        Task<List<RandomThoughtsCategoryLookup>> GetRandomThoughtsCategoryLookupsAsync();

        RandomThought GetRandomThought(int postId);

        string GetRandomThoughtText(string filePath);








    }
}
