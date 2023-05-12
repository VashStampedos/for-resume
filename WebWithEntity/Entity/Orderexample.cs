using System;
using System.Collections.Generic;

namespace WebWithEntity
{
    public partial class Orderexample
    {
        public int Id { get; set; }
        public int? IdOrder { get; set; }
        public int? IdExample { get; set; }
        public int? Count { get; set; }

        public virtual Examples IdExampleNavigation { get; set; }
        public virtual Orders IdOrderNavigation { get; set; }
    }
}
