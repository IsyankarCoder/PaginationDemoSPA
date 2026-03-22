using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PaginationDemoSPA.Data;
using PaginationDemoSPA.Helper;
using PaginationDemoSPA.Models;

namespace PaginationDemoSPA.Service
{
    public class ProductService
    {

        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  (List<Product> Products, int TotalPages) --> Tupple 
        ///  List<Product> Products → Ürün listesi
        ///  int TotalPages → Toplam sayfa sayısı
        ///  Parantez → Bunları tek bir "paket" (tuple) olarak döndürüyoruz
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<(List<Product> Products, int TotalPages)>> GetPagedProductsAsync(int page,
                                                                                                  int pageSize,
                                                                                                  CancellationToken cancellationToken = default,
                                                                                                  string search="",
                                                                                                  decimal? minPrice=null,
                                                                                                  decimal? maxPrice=null
                                                                                                  )
        {
            try
            {
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(x => x.Name.Contains(search));

                if (minPrice.HasValue)
                    query = query.Where(x => x.Price >= minPrice);

                if (maxPrice.HasValue)
                    query = query.Where(x => x.Price <= maxPrice);


                var totalProducts = await query.CountAsync(cancellationToken);
                if (totalProducts == 0)
                    return Result<(List<Product>, int)>.Fail("Hiç ürün buluanamadı");

                var products = await  query
                                             .OrderBy(d => d.Id)
                                             .Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                return Result<(List<Product>, int)>.Ok((products, totalPages));

            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                return Result<(List<Product>, int)>.Fail($"Bir hata oluştu: {ex.Message}");
            }

        }
    }
}
