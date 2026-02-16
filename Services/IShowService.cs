using DTOs;

namespace Services
{
    public interface IShowService
    {
        Task<ShowReadDTO> addShow(ShowCreateDTO showCDTO);
        Task<List<ShowReadDTO>> getAllShows();
        Task<(IEnumerable<ShowReadDTO> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[] categoryId, string[] sectors, string[] audiences);
        Task<ShowReadDTO> getShowById(int id);
        Task<ShowReadDTO> updateShow(ShowUpdateDTO showUDTO, int id);
    }
}