using Entities;

namespace Repositories
{
    public interface IShowsRepository
    {
        Task<Show> addShow(Show show);
        Task<List<Show>> getAllShows();
        Task<(IEnumerable<Show> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[] categoryId, string[] sectors, string[] audiences);
        Task<Show> getShowById(int id);
        Task<Show> updateOrder(Show show, int id);
    }
}