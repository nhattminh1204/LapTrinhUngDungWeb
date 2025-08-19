using Microsoft.AspNetCore.Mvc;
using Product_Management.DTO;
using Product_Management.Models;
using Product_Management.ViewModel;

namespace Product_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 5)
        {
            var model = new CategoryViewModel
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItem = _context.Categories.Count(),
            };

            model.Categories = _context.Categories
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(e => new CategoryDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToList();
            return View(model);
        }

        public IActionResult LoadCategory(string keyword = "", int pageIndex = 1, int pageSize = 5)
        {
            IQueryable<Category> query = _context.Categories;
            
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(e => e.Name.Contains(keyword));
            }

            var model = new CategoryViewModel
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItem = query.Count(),
            };

            model.Categories = query
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(e => new CategoryDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToList();

            return PartialView("_List", model);
        }


        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            var category = new Category
            {
                Name = model.Request.Name,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            
            var model = new CategoryViewModel
            {
                Response = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                }
            };
            return PartialView("_Update", model);
        }

        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var category = _context.Categories.Find(model.Request.Id);
            if (category == null) return NotFound();

            category.Name = model.Request.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetCategoryProducts(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            var products = _context.Products
                .Where(p => p.CategoryId == id)
                .Select(p => p.Name)
                .ToList();

            return Json(new { categoryName = category.Name, products = products });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            // Xóa tất cả sản phẩm thuộc danh mục
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (products.Any())
            {
                // Xóa ảnh của các sản phẩm
                foreach (var product in products)
                {
                    if (!string.IsNullOrEmpty(product.image))
                    {
                        var imagePath = Path.Combine(_environment.WebRootPath, product.image.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                }
                _context.Products.RemoveRange(products);
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
