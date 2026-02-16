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

        public async Task<(IEnumerable<Show> shows, int total)> getAllShows( string description, int? minPrice, int? maxPrice, int skip, int position,int[]? categoryId)
        {
            var query = _context.Shows.Where(show =>
                        (description == null ? (true) : (show.Description.Contains(description)))
                        && ((minPrice == null) ? (true) : (minShowPrice(show) >= minPrice))
                        && ((maxPrice == null) ? (true) : (maxShowPrice(show) <= maxPrice))
                        && (categoryId == null || categoryId.Length == 0 || categoryId.Contains(show.CategoryId)))
                            .OrderBy(show => minShowPrice(show));

            Console.WriteLine(query.ToQueryString());
            List<Show> shows = await query.Skip((position - 1) * skip)
            .Take(skip).Include(show => show.Category).ToListAsync();
            var total = await query.CountAsync();
            return (shows, total);
        }

        public int minShowPrice(Show show)
        {
            int minPrice = int.MaxValue;
            foreach (var section in show.Sections)
            {
                if(section.Price < minPrice)
                {
                    minPrice = (int)section.Price;
                }
            }
            return minPrice;
        }

        public int maxShowPrice(Show show)
        {
            int maxPrice = int.MinValue;
            foreach (var section in show.Sections)
            {
                if (section.Price < maxPrice)
                {
                    maxPrice = (int)section.Price;
                }
            }
            return maxPrice;
        }

        //public async Task deleteShow(int id)
        //{
        //        await _context.Shows.ExecuteDeleteAsync(s => s.Id == id);
        //        await _context.SaveChangesAsync();
        //}
    }
}
