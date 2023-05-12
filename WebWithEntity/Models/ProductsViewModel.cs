using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Models
{
    public class ProductsViewModel
    {
        public ProductsViewModel()
        {

        }
        public IEnumerable<Products> Products { get; set; }
        public PageViewModel PageModel { get; set; }
        public FilterViewModel FilterModel { get; set; }
        public SortViewModel SortModel { get; set; }
    }

    public enum Genders
    {
        [Display(Name = "любой")]
        Any,
        [Display(Name = "мужской")]
        Man,
        [Display(Name = "женский")]
        Woman,
        [Display(Name = "унисекс")]
        Unisex
       
    }

    public enum Sizes
    {
        [Display(Name = "Size 1")]
        Size_1,
        [Display(Name = "Size 2")]
        Size_2,
        [Display(Name = "Size 3")]
        Size_3
    }

    public enum SortState
    {
        [Display(Name = "Сначала старые")]
        NewnessAsc,
        [Display(Name = "Сначала новые")]
        NewnessDesc,
        [Display(Name = "Название: по возрастанию")]
        NameAsc,
        [Display(Name = "Название: по убыванию")]
        NameDesc,
        [Display(Name = "Цена: по возрастанию")]
        PriceAsc,
        [Display(Name = "Цена: по убыванию")]
        PriceDesc
    }
}
