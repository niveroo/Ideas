using Ideas.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ideas.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductReviewContext _context;

        public ProductsController(ProductReviewContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProduct()
        {
            var products = _context.Products.ToList();

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return products;
        }
    }
}
