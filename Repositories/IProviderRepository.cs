using Entities;

namespace Repositories
{
    public interface IProviderRepository
    {
        Task<Provider> addProvider(Provider provider);
        Task<List<Provider>> getAllProviders();
        Task<Provider> getProviderById(int id);
    }
}