using Entities;

namespace Services
{
    public interface IProviderService
    {
        Task<ProviderReadDTO> addProvider(ProviderCreateDTO provider);
        Task<List<ProviderReadDTO>> getAllProviders();
        Task<ProviderReadDTO> getProviderById(int id);
    }
}