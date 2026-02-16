using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ShowsRepository : IShowsRepository
    {
        ShowsCenterContext _context;
        public ShowsRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }
        //to add filters, sorting, and pagination!!!!!
        public async Task<List<Show>> getAllShows()
        {
            return await _context.Shows.Include(s=>s.Provider).Include(s=>s.Category).ToListAsync();
        }
        public async Task<Show> getShowById(int id)
        {
            return await _context.Shows.Include(s=>s.Provider).Include(s=>s.Category).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Show> addShow(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();
            if (getShowById(show.Id) != null)
                return show;
            else
                return null;
        }
        public async Task<Show> updateOrder(Show show, int id)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
            return show;
        }
        //public async Task deleteShow(int id)
        //{
        //        await _context.Shows.ExecuteDeleteAsync(s => s.Id == id);
        //        await _context.SaveChangesAsync();
        //}
    }
}
