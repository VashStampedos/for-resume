using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Models
{
    public class FilterViewModel
    {
        
        public IEnumerable<Products> Products { get; set; }
        public SelectList Categories { get; set; }
        public int? SelectedCategory { get; set; }
        public Genders? Gender { get; set; }
        public int? MaxPriceValue { get; set; }
        public int? MinPriceValue { get; set; }
        public string Name { get; set; }
        public FilterViewModel(List<Categories> categories, int? category,  Genders? gender, int? maxPrice, int? minPrice, string name)
        {
            categories.Insert(0, new Categories { CategoryName = "Все", Id = 0 });
            Categories = new SelectList(categories, "Id", "CategoryName", category);
            SelectedCategory = category;
            Gender = gender;
            MaxPriceValue = maxPrice;
            MinPriceValue = minPrice;
            Name = name;

        }
    }
}
