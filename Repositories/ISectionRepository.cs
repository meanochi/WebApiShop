using Entities;

namespace Repositories
{
    public interface ISectionRepository
    {
        Task<List<Section>> getSectionsByShowId(int showId);
    }
}