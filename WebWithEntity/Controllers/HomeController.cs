using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebWithEntity.Entity;
using WebWithEntity.Models;

namespace WebWithEntity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Tshop2Context db;
        FavoriteViewModel favoriteModel = new FavoriteViewModel();
        public HomeController(Tshop2Context context)
        {
            db = context;
        }
        public IActionResult Office()
        {
            if (User.Identity.IsAuthenticated)
            {
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                var orders = db.Orderexample.Include(x => x.IdOrderNavigation).ThenInclude(x=> x.IdUserNavigation).Where(x=> x.IdOrderNavigation.IdUser == idUser.Id)
                    .Include(x=> x.IdExampleNavigation).ThenInclude(x=>x.IdProductNavigation).ThenInclude(x=> x.IdCategoryNavigation);
                var favorites = db.Favorites.Include(x => x.IdProductNavigation).ThenInclude(x => x.IdCategoryNavigation)
                    .Include(x => x.IdUserNavigation).Where(x => x.IdUser == idUser.Id);
                OfficeViewModel viewModel = new OfficeViewModel
                {
                    MyOrders = orders,
                    Favorites = favorites
                };
                return View(viewModel);
            }
            return RedirectToAction("Login", "Account");

        }
        [HttpGet]
        public IActionResult GetFavorites()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                var myFavorites = db.Favorites.Include(x => x.IdUserNavigation).Include(x => x.IdProductNavigation).ThenInclude(x => x.IdCategoryNavigation).Where(x => x.IdUser == idUser.Id);
                return View("MyFavorites", myFavorites);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult GetOrders(IEnumerable<Orderexample> orders)
        {
            return View("MyOrders", orders);
        }

        [HttpGet]
        public IActionResult AddInFavorites(int id, int? category, Genders? gender, int? maxPrice, int? minPrice, string name, int page = 1, SortState sortmodel = SortState.NewnessAsc)
        {
            if (User.Identity.IsAuthenticated)
            {
                var viewModel = InitialViewModel(category, gender, maxPrice, minPrice, name, page, sortmodel);
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                var favoriteProduct = db.Products.FirstOrDefault(x => x.Id == id);
                    var newFavorite = new Favorites { IdProduct = favoriteProduct.Id, IdUser=idUser.Id };
                if(db.Favorites.Any(x=> x.IdProduct == newFavorite.IdProduct && x.IdUser == newFavorite.IdUser))
                {
                    return View("Index", viewModel);
                }
                db.Favorites.Add(newFavorite);
                db.SaveChanges();
                return View("Index", viewModel);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult RemoveFromFavorites(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("GetFavorites");
            }
            if (User.Identity.IsAuthenticated)
            {
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                var removeProd = db.Favorites.First(x => x.Id == id && x.IdUser == idUser.Id);
                db.Favorites.Remove(removeProd);
                db.SaveChanges();
                return RedirectToAction("GetFavorites");
            }
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public IActionResult AddInBasket(int? id, int? idprod, Sizes? size,  int count=1)
        {
            
            if (User.Identity.IsAuthenticated)
            {
                Examples example;
                if (idprod != null && size.HasValue)
                {
                    var newSize = "";
                    newSize = size switch
                    {
                        Sizes.Size_1 => "size1",
                        Sizes.Size_2 => "size2",
                        Sizes.Size_3 => "size3",

                    };
                    example = db.Examples.FirstOrDefault(x => x.IdProduct == idprod && x.Size == newSize);
                }
                else
                {
                    example = db.Examples.FirstOrDefault(x => x.Id == id);
                }
                var idUser =db.Users.FirstOrDefault(x=> x.Login== User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                if(!db.Baskets.Any(x=> x.IdExample == example.Id && x.IdUser == idUser.Id))
                {
                    db.Baskets.Add(new Baskets { Id = 0, IdExample = example.Id, IdUser = idUser.Id, Counts = count });
                    db.SaveChanges();

                }
                return RedirectToAction("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
                .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
            }
         
            return RedirectToAction("Login", "Account");
        }

        public IActionResult PlusCountProductInBasket(int id)// сделать имзенение кол-ва в коризне
        {
            var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
            Baskets basprod = db.Baskets.FirstOrDefault(i => i.Id==id);
            basprod.Counts++;
            db.Baskets.Update(basprod);
            db.SaveChanges();
            return RedirectToAction("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
                .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
        }
        public IActionResult MinusCountProductInBasket(int id)
        {
            var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
            Baskets basprod = db.Baskets.FirstOrDefault(i => i.Id == id);
            if (basprod.Counts <= 1) {
                return RedirectToAction("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
                .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
            }
            basprod.Counts--;
            db.Baskets.Update(basprod);
            db.SaveChanges();
            return RedirectToAction("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
                .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
        }

        public IActionResult RemoveFromBasket(int? id)
        {
            
            if (id == null)
            {
                return RedirectToAction("Basket");
            }
            if (User.Identity.IsAuthenticated)
            {
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                db.Baskets.Remove(db.Baskets.First(x => x.Id == id && x.IdUser==idUser.Id));
                db.SaveChanges();
                return RedirectToAction("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
                .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
            }
            return RedirectToAction("Login", "Account");

        }

        public IActionResult Basket()
        {
            if (User.Identity.IsAuthenticated)
            {
                var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
                return View("Basket", db.Baskets.Include(x => x.IdExampleNavigation).ThenInclude(y => y.IdProductNavigation).ThenInclude(z => z.IdCategoryNavigation)
               .Include(r => r.IdUserNavigation).Where(i => i.IdUser == idUser.Id));
            }
            return RedirectToAction("Login", "Account");
        }

        

        [AllowAnonymous]
        public IActionResult Index(int? category, Genders? gender, int? maxPrice, int? minPrice, string name, int page = 1, SortState sortmodel = SortState.NewnessAsc)
        {
            var viewModel = InitialViewModel(category, gender, maxPrice, minPrice, name, page, sortmodel);
            return View(viewModel); 
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            return View(db.Examples.Include(x=>x.IdProductNavigation).ThenInclude(x => x.IdCategoryNavigation).Where(x => x.IdProduct == id)); 
        }
        [HttpPost]
        public IActionResult Buy(string city, string street, string house, string flat)
        {
            Orders order = new Orders(); 
            order.Id = 0;
            order.DateOrder = DateTime.Now;
            string adress = city + " " + street + " " + house + " " + flat;
            order.Adress = adress;
            order.Status = "nedostavlen";
            var idUser = db.Users.FirstOrDefault(x => x.Login == User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
            order.IdUser = idUser.Id;
            order.IdUserNavigation = db.Users.First(x => x.Id == idUser.Id);
            db.Orders.Add(order);
            db.SaveChanges();
            var exampels = db.Examples.Include(x => x.IdProductNavigation);
            foreach (var item in db.Baskets.Where(x=>x.IdUser==idUser.Id))
            {
                order.Orderexample.Add(new Orderexample { IdOrder = order.Id, IdExample = item.IdExample, Count = item.Counts });
            }
            db.Baskets.RemoveRange(db.Baskets.Where(x => x.IdUser == idUser.Id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ProductsViewModel InitialViewModel(int? category, Genders? gender, int? maxPrice, int? minPrice, string name, int page = 1, SortState sortmodel = SortState.NewnessAsc, int pageSize = 8)
        {

            IQueryable<Products> products = db.Products.Include(x => x.IdCategoryNavigation);
            if (category != null && category != 0)
            {
                products = products.Where(x => x.IdCategory == category);
            }
            if ((maxPrice != null && maxPrice != 0) && (minPrice == null || minPrice == 0))
            {
                products = products.Where(x => x.Price <= maxPrice);
            }
            else if ((minPrice != null && minPrice != 0) && (maxPrice == null || maxPrice == 0))
            {
                products = products.Where(x => x.Price >= minPrice);
            }
            else if ((minPrice != null && minPrice != 0) && (maxPrice != null && maxPrice != 0))
            {
                products = products.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
            }
            if (gender.HasValue)
            {
                products = gender switch
                {
                    Genders.Any => products.Where(g => g.Gender == "мужской" || g.Gender == "женский" || g.Gender == "унисекс"),
                    Genders.Man => products.Where(g => g.Gender == "мужской"),
                    Genders.Woman => products.Where(g => g.Gender == "женский"),
                    Genders.Unisex => products = products.Where(g => g.Gender == "унисекс")
                };
            }
            if (!String.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.ProductNname.Contains(name));
            }
            products = sortmodel switch
            {
                SortState.NewnessDesc => products.OrderByDescending(i => i.Id),
                SortState.NewnessAsc => products.OrderBy(i => i.Id),
                SortState.NameDesc => products.OrderByDescending(i => i.ProductNname),
                SortState.NameAsc => products.OrderBy(i => i.ProductNname),
                SortState.PriceDesc => products.OrderByDescending(i => i.Price),
                SortState.PriceAsc => products.OrderBy(i => i.Price),
            };
            var count = products.Count();
            var items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var viewModel = new ProductsViewModel
            {
                PageModel = new PageViewModel(count, page, pageSize),
                SortModel = new SortViewModel(sortmodel),
                FilterModel = new FilterViewModel(db.Categories.ToList(), category, gender, maxPrice, minPrice, name),
                Products = items.ToList(),
            };
            favoriteModel.ProductModel = viewModel;
            return viewModel;
        }
    }
}
