using System;
using System.Collections.Generic;

namespace WebWithEntity
{
    public partial class Orders
    {
        public Orders()
        {
            Orderexample = new HashSet<Orderexample>();
        }

        public int Id { get; set; }
        public DateTime? DateOrder { get; set; }
        public string Status { get; set; }
        public int? IdUser { get; set; }
        public string Adress { get; set; }

        public virtual Users IdUserNavigation { get; set; }
        public virtual ICollection<Orderexample> Orderexample { get; set; }
    }
}
