using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderItemRepository
    {
        ShowsCenterContext _context;
        public OrderItemRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }


    }
}
