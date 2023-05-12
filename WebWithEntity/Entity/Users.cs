using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebWithEntity.Entity;
using WebWithEntity.Models;

namespace WebWithEntity
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public bool ConfirmedEmail { get; set; }
        public bool? IsAdmin { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<Baskets> Baskets { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
