using System;
using System.Collections.Generic;
using WebWithEntity.Entity;

namespace WebWithEntity
{
    public partial class Products
    {
        public Products()
        {
            Examples = new HashSet<Examples>();
        }

        public int Id { get; set; }
        public string ProductNname { get; set; }
        public string Gender { get; set; }
        public int? IdCategory { get; set; }
        public string Photo { get; set; }
        public int Price { get; set; }

        public virtual Categories IdCategoryNavigation { get; set; }
        public virtual ICollection<Examples> Examples { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
    }
}
