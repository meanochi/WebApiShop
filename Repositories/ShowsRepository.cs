using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            return await _context.Shows.Include(s=>s.Provider).Include(s=>s.Category).Include(s=>s.Sections).FirstOrDefaultAsync(o => o.Id == id);
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

        public async Task<(IEnumerable<Show> shows, int total)> getAllShows(ShowFilterDTO filters)
        {

            var query = _context.Shows.Where(show =>
                        (filters.description == null ? (true) : (show.Title.Contains(filters.description)))
                        && ((filters.minPrice == null) ? (true) : (show.Sections.Min(s=>s.Price) >= filters.minPrice))
                        && ((filters.maxPrice == null) ? (true) : (show.Sections.Max(s => s.Price) <= filters.maxPrice))
                        && (filters.categoryIdS == null || filters.categoryIdS.Length == 0 || filters.categoryIdS.Contains(show.CategoryId))
                        && (filters.audiences == null || filters.audiences.Length == 0 || filters.audiences.Contains(show.Audience))
                        && (filters.sectors == null || filters.sectors.Length == 0 || filters.sectors.Contains(show.Sector)))
                            .OrderBy(show => show.Sections.Min(s => s.Price)).Include(s => s.Provider).Include(s => s.Category);

            //Console.WriteLine(query.ToQueryString());
            List<Show> shows = await query.Skip((filters.position - 1) * filters.skip)
            .Take(filters.skip)
            .Include(show => show.Category)
            .Include(show => show.Sections)
            .ToListAsync();
            var total = await query.CountAsync();
            return (shows, total);
        }


        //public async Task deleteShow(int id)
        //{
        //        await _context.Shows.ExecuteDeleteAsync(s => s.Id == id);
        //        await _context.SaveChangesAsync();
        //}
    }
}
