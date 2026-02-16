using DTOs;

namespace Services
{
    public interface IShowService
    {
        Task<ShowReadDTO> addShow(ShowCreateDTO showCDTO);
        Task<List<ShowReadDTO>> getAllShows();
<<<<<<< HEAD
        Task<(IEnumerable<ShowReadDTO> shows, int total)> getAllShows(string description, int? minPrice, int? maxPrice, int skip, int position, int[]? categoryId);
=======
        Task<(IEnumerable<ShowReadDTO> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[]? categoryId);
>>>>>>> 98b507ee68bf3ba21c9035054969fe530f855075
        Task<ShowReadDTO> getShowById(int id);
        Task<ShowReadDTO> updateShow(ShowUpdateDTO showUDTO, int id);
    }
}