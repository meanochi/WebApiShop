using DTOs;

namespace Services
{
    public interface IShowService
    {
        Task<ShowReadDTO> addShow(ShowCreateDTO showCDTO);
        Task<List<ShowReadDTO>> getAllShows();
        Task<ShowReadDTO> getShowById(int id);
        Task<ShowReadDTO> updateShow(ShowUpdateDTO showUDTO, int id);
    }
}