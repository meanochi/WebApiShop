using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        ShowsCenterContext _context;
        public ProviderRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }
        public async Task<Provider> getProviderById(int id)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Provider>> getAllProviders()
        {
            return await _context.Providers.ToListAsync();
        }

        public async Task<Provider> addProvider(Provider provider)
        {
            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();
            var saved = await getProviderById(provider.Id); 
            return saved != null ? provider : null;
        }
    }
}
