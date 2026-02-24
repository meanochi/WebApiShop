using DTOs;

namespace Services
{
    public interface ISectionService
    {
        Task<List<SectionReadDTO>> getSectionsByShowId(int showId);
    }
}