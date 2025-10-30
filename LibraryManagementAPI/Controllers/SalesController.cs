using LibraryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // Query 1: Calculate total quantity_sold
        [HttpGet("total-quantity-sold")]
        public async Task<IActionResult> GetTotalQuantitySold()
        {
            var totalQuantity = await _context.Sales.SumAsync(s => s.QuantitySold);

            return Ok(new
            {
                query = "SELECT SUM(quantity_sold) AS TotalQuantitySold FROM Sales",
                result = new { TotalQuantitySold = totalQuantity }
            });
        }

        // Query 2: Product with highest unit price
        [HttpGet("highest-priced-product")]
        public async Task<IActionResult> GetHighestPricedProduct()
        {
            // Load all products into memory first (client-side evaluation)
            var products = await _context.Products.ToListAsync();

            // Now we can safely use LINQ to Objects
            var product = products
                .OrderByDescending(p => p.UnitPrice)
                .Select(p => new { p.ProductName, p.UnitPrice })
                .FirstOrDefault();

            return Ok(new
            {
                query = "SELECT product_name, unit_price FROM Products ORDER BY unit_price DESC LIMIT 1",
                result = product
            });
        }

        // Query 3: Products not sold
        [HttpGet("products-not-sold")]
        public async Task<IActionResult> GetProductsNotSold()
        {
            var productsNotSold = await _context.Products
                .Where(p => !_context.Sales.Any(s => s.ProductId == p.ProductId))
                .Select(p => new { p.ProductId, p.ProductName, p.Category, p.UnitPrice })
                .ToListAsync();

            return Ok(new
            {
                query = "SELECT p.* FROM Products p LEFT JOIN Sales s ON p.product_id = s.product_id WHERE s.sale_id IS NULL",
                result = productsNotSold
            });
        }

        // Bonus: Get all sales with product details
        [HttpGet("all-sales")]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _context.Sales
                .Include(s => s.Product)
                .Select(s => new
                {
                    s.SaleId,
                    s.ProductId,
                    s.Product.ProductName,
                    s.QuantitySold,
                    s.SaleDate,
                    s.TotalPrice
                })
                .ToListAsync();

            return Ok(sales);
        }
    }
}