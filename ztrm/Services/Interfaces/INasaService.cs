using ztrm.Models.DTOs;

namespace ztrm.Services
{
    public interface INasaService
    {

        Task<ApodDto?> GetApodAsync(DateOnly? date = null, bool hd = true, bool thumbs = true, CancellationToken ct = default);

    }
}
