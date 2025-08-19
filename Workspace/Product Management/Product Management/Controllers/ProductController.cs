using Microsoft.AspNetCore.Mvc;
using Product_Management.Models;
using Product_Management.ViewModel;

namespace Product_Management.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;

        public ProductController(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 5)
        {
            var model = new ProductViewModel
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItem = _context.Products.Count(),
            };


            model.Categories = _context.Categories.Select(e => new DTO.CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).ToList();


            model.Products = _context.Products
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(e => new DTO.ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    image = e.image,
                    Description = e.Description,
                    ManuDate = e.ManuDate,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name
                }).ToList();
            return View(model);
        }

        public IActionResult LoadProduct(string keyword = "", int idCategory = 0,
            int pageIndex = 1, int pageSize = 5)
        {
            IQueryable<Product> query = _context.Products;
            if (idCategory != 0)
            {
                query = query.Where(e => e.CategoryId == idCategory);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(e => e.Name.Contains(keyword));
            }

            var model = new ProductViewModel
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItem = query.Count(),
            };


            model.Categories = _context.Categories.Select(e => new DTO.CategoryDTO
            {
                Id = e.Id,
                Name = e.Name,
            }).ToList();


            model.Products = query
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(e => new DTO.ProductDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    image = e.image,
                    Description = e.Description,
                    ManuDate = e.ManuDate,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name
                }).ToList();

            return PartialView("_List", model);
        }

        public IActionResult Create(ProductViewModel model)
        {
            var product = new Product
            {
                Name = model.Request.Name,
                CategoryId = model.Request.CategoryId,
                Price = model.Request.Price,
                Quantity = model.Request.Quantity,
                ManuDate = model.Request.ManuDate,
                Description = model.Request.Description ?? "",
            };

            if (model.Request.formFileImage != null)
            {
                // Lưu vào thư mục
                var folder = Path.Combine(_enviroment.WebRootPath, "images");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Request.formFileImage.FileName);
                var fullFileName = Path.Combine(folder, fileName);

                using (var fileStream = new FileStream(fullFileName, FileMode.Create))
                {
                    model.Request.formFileImage.CopyTo(fileStream);
                }
                product.image = $"/images/{fileName}";
                // Lưu vào db

            }
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            
            var model = new ProductViewModel
            {
                Categories = _context.Categories.Select(e => new DTO.CategoryDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToList(),
                Response = new DTO.ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ManuDate = product.ManuDate,
                    CategoryId = product.CategoryId,
                    image = !string.IsNullOrEmpty(product.image) && !product.image.StartsWith("/") ? $"/images/{product.image}" : product.image
                }
            };
            return PartialView("_Update", model);
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            var product = _context.Products.Find(model.Request.Id);
            if (product == null) return NotFound();

            // Cập nhật thông tin cơ bản
            product.Name = model.Request.Name;
            product.Description = model.Request.Description ?? "";
            product.Price = model.Request.Price;
            product.Quantity = model.Request.Quantity;
            product.ManuDate = model.Request.ManuDate;
            product.CategoryId = model.Request.CategoryId;

            // Xử lý ảnh mới
            if (model.Request.formFileImage != null)
            {
                // Xóa ảnh cũ
                if (!string.IsNullOrEmpty(product.image))
                {
                    var oldImagePath = Path.Combine(_enviroment.WebRootPath, product.image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Lưu ảnh mới
                var folder = Path.Combine(_enviroment.WebRootPath, "images");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Request.formFileImage.FileName);
                var fullFileName = Path.Combine(folder, fileName);

                using (var fileStream = new FileStream(fullFileName, FileMode.Create))
                {
                    model.Request.formFileImage.CopyTo(fileStream);
                }
                product.image = $"/images/{fileName}";
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            // Xóa ảnh nếu có
            if (!string.IsNullOrEmpty(product.image))
            {
                var imagePath = Path.Combine(_enviroment.WebRootPath, product.image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
