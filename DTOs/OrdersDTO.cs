using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record OrdersDTO
(
     int OrderId,

     DateOnly? OrderDate,

     double? OrderSum,

     int UserId//,

    // string UserName

 );

}
