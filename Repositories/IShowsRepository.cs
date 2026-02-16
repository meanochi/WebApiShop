using Entities;

namespace Repositories
{
    public interface IShowsRepository
    {
        Task<Show> addShow(Show show);
        Task<List<Show>> getAllShows();
        Task<Show> getShowById(int id);
        Task<Show> updateOrder(Show show, int id);
    }
}