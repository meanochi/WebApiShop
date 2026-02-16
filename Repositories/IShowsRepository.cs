using Entities;

namespace Repositories
{
    public interface IShowsRepository
    {
        Task<Show> addShow(Show show);
        Task<List<Show>> getAllShows();
<<<<<<< HEAD
        Task<(IEnumerable<Show> shows, int total)> getAllShows(string description, int? minPrice, int? maxPrice, int skip, int position, int[]? categoryId);
=======
        Task<(IEnumerable<Show> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[]? categoryId);
>>>>>>> 98b507ee68bf3ba21c9035054969fe530f855075
        Task<Show> getShowById(int id);
        Task<Show> updateOrder(Show show, int id);
    }
}