using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Entity
{
    public partial class Baskets
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdExample { get; set; }
        public int Counts{ get; set; }
        public virtual Users IdUserNavigation { get; set; }
        public virtual Examples IdExampleNavigation { get; set; }
    }
}
