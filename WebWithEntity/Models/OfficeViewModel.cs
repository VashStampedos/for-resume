using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWithEntity.Entity;

namespace WebWithEntity.Models
{
    public class OfficeViewModel
    {
        public OfficeViewModel()
        {

        }
        public IEnumerable<Orderexample> MyOrders { get; set; }

        public IEnumerable<Favorites> Favorites { get; set; }
    }
}
