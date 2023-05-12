using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Models
{
    public class SortViewModel
    {
        public SortState NewnessSort { get; set; }
        public SortState NameSort { get; set; }
        public SortState PriceSort { get; set; }
        public SortState Current { get; set; }
        //public bool Up { get; set; }

        public SortViewModel(SortState sortmodel)
        {
            NewnessSort = sortmodel == SortState.NewnessAsc ? SortState.NewnessDesc : SortState.NewnessAsc;
            NameSort = sortmodel == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            PriceSort = sortmodel == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            Current = sortmodel;
        }
    }

  
}
