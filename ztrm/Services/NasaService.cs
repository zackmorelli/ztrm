using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ztrm.Models;
using ztrm.Models.DTOs;

namespace ztrm.Services
{
    public class NasaService : INasaService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _http;
        private readonly string _key;
        private readonly IMemoryCache _cache;

        public NasaService(ILogger<NasaService> logger, HttpClient http, IOptions<NasaOptions> opt, IMemoryCache cache)
        {
            _logger = logger;
            _http = http;
            _key = opt.Value.ApiKey;
            _cache = cache;
        }

        public async Task<ApodDto?> GetApodAsync(DateOnly? date = null, bool hd = true, bool thumbs = true, CancellationToken ct = default)
        {
            DateOnly day = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
            string cacheKey = $"apod:{day:yyyy-MM-dd}:{hd}:{thumbs}";

            if (_cache.TryGetValue(cacheKey, out ApodDto? cached))
                return cached;

            string method = nameof(GetApodAsync);
            try
            {
                var url = $"planetary/apod?api_key={_key}" +
                          (hd ? "&hd=true" : "") +
                          (thumbs ? "&thumbs=true" : "");

                _logger.LogInformation("{Method} requesting {Url}", method, url);

                ApodDto apod = await _http.GetFromJsonAsync<ApodDto>(url, ct);

                // Cache until next UTC midnight (APOD rotates daily)
                var nextUtcMidnight = DateTimeOffset.UtcNow.Date.AddDays(1);
                _cache.Set(cacheKey, apod, nextUtcMidnight);

                _logger.LogInformation("{Method} success for {Date}", method, day);
                return apod;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("{Method} canceled for {Date}", method, day);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} failed for {Date}", method, day);
                return null; // keep service boundary soft; PageModel decides UX
            }
        }

    }
}
