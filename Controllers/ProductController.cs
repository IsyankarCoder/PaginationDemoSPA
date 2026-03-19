using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginationDemoSPA.Data;

namespace PaginationDemoSPA.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDbContext _appDbContext;
        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            int pageSize = 10;

            var totalProducts = await _appDbContext.Products.CountAsync();

            var products = await _appDbContext
                           .Products
                           .OrderBy(d => d.Id)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync(cancellationToken);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);


            return View(products);
        }

        //Partialview for Ajax
        public async Task<IActionResult> GetProductsPartial(int page = 1, CancellationToken cancellationToken = default)
        {
            int pageSize = 10;

            var totalProducts = await _appDbContext.Products.CountAsync();

            var products = await _appDbContext.Products
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);


            return PartialView("_ProductsTable", products);

        }
    }
}
