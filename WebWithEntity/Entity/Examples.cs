using System;
using System.Collections.Generic;
using WebWithEntity.Entity;

namespace WebWithEntity
{
    public partial class Examples
    {
        public Examples()
        {
            Orderexample = new HashSet<Orderexample>();
        }

        public int Id { get; set; }
        public int? IdProduct { get; set; }
        public string Size { get; set; }

        public virtual Products IdProductNavigation { get; set; }
        public virtual ICollection<Orderexample> Orderexample { get; set; }

        public virtual ICollection<Baskets> Baskets { get; set; }
       
    }
}
