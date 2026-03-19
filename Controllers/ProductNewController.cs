using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginationDemoSPA.Data;
using PaginationDemoSPA.Models;
using PaginationDemoSPA.Service;

namespace PaginationDemoSPA.Controllers
{
    public class ProductNewController 
        : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly ProductService _productService;
        public ProductNewController(ProductService productService, AppDbContext appDbContext)
        {
            _productService = productService;
            _appDbContext = appDbContext;
        }


        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            int pageSize = 10;

            var result = await _productService.GetPagedProductsAsync(page, pageSize, cancellationToken);
            if (!result.Success)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = Guid.NewGuid().ToString(),
                });
            }

            var (products, totalpages) = result.Data;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalpages;

            return View(products);

        }


        //Partialview for Ajax
        public async Task<IActionResult> GetProductsPartial(int page = 1, CancellationToken cancellationToken = default)
        {
            int pageSize = 10;

            var result = await _productService.GetPagedProductsAsync(page, pageSize, cancellationToken);
            if (!result.Success)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = Guid.NewGuid().ToString(),
                });
            }

            var (products, totalpages) = result.Data;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalpages;


            return PartialView("_ProductsTable", products);

        }
    }
}
