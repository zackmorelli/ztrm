namespace ztrm.Models.DTOs
{
    public record ApodDto
    (
        string date,
        string title,
        string explanation,
        string media_type,
        string url,
        string? hdurl = null,
        string? thumbnail_url = null,
        string? copyright = null
    );
}
