using Ideas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        [HttpGet("get-top-rated-products")]
        public IActionResult GetTopRatedProducts()
        {
            var connection = (NpgsqlConnection)_context.Database.GetDbConnection();

            try
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM get_top_rated_products()", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var topRatedProducts = new List<ProductRating>();

                        while (reader.Read())
                        {
                            var product = new ProductRating
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                AverageRating = reader.GetDouble(reader.GetOrdinal("average_rating"))
                            };

                            topRatedProducts.Add(product);
                        }

                        if (topRatedProducts.Any())
                        {
                            return Ok(topRatedProducts);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


    }
}
