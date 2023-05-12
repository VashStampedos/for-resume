using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Entity
{
    public partial class Favorites
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdProduct { get; set; }
        public virtual Users IdUserNavigation { get; set; }
        public virtual Products IdProductNavigation { get; set; }

    }
}
