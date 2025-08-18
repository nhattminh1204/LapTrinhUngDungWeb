using Microsoft.AspNetCore.Mvc;
using Product_Management.DTO;
using Product_Management.Models;
using Product_Management.ViewModel;

namespace Product_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword = "", int pageSize = 5, int pageIndex = 1)
        {
            var model = new CategoryViewModel();
            model.PageSize = pageSize;
            model.PageIndex = pageIndex;

            IQueryable<Category> list = _context.Categories;

            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(e => e.Name.Contains(keyword));
                
            }
            model.TotalItem = list.Count();
            model.Categories = list
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).ToList();

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            var cate = new Category();
            cate.Name = model.Request.Name;
            _context.Categories.Add(cate);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var cat = _context.Categories.Where(e => e. Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).FirstOrDefault();
            var CategoryViewModel = new CategoryViewModel();
            CategoryViewModel.Response = cat;
            return View(CategoryViewModel);
        }

        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var cat = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (cat != null)
            {
                cat.Name = model.Request.Name;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]        
        public IActionResult Delete(int id) 
        {
            var cat = _context.Categories.Where(e => e.Id == id).Select(e => new CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).FirstOrDefault();
            var CategoryViewModel = new CategoryViewModel();
            CategoryViewModel.Response = cat;
            return View(CategoryViewModel);
        }

        [HttpPost]
        public IActionResult Delete(CategoryViewModel model)
        {   
            var cat = _context.Categories.Where(e => e.Id == model.Request.Id).FirstOrDefault();
            if (cat != null)
            {
                var products = _context.Products.Where(e => e.CategoryId == model.Request.Id).ToList();
                if (products.Count > 0)
                {
                    _context.Products.RemoveRange(products);
                }
                _context.Categories.Remove(cat);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
