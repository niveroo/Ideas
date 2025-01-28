using Ideas.Models;
using Ideas.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SMSApi.Api.Action;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Ideas.Controllers.Reviws
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ProductReviewContext _context;
        private readonly SmsSender _smsSender;

        public ReviewsController(ProductReviewContext context, SmsSender smsSender)
        {
            _context = context;
            _smsSender = smsSender;
        }

        [HttpPost("AddReview")]
        public ActionResult<Review> AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            _smsSender.SendPromocode(review.PhoneNumber);

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpGet("{id}")]
        public ActionResult<Review> GetReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpGet("ByPhoneNumber")]
        public ActionResult<IEnumerable<Review>> GetReviewsByPhoneNumber(string phoneNumber)
        {
            var reviews = _context.Reviews.Where(r => r.PhoneNumber == phoneNumber).ToList();

            if (reviews == null || !reviews.Any())
            {
                return NotFound();
            }

            return reviews;
        }

        [HttpGet("get-average-rating/{productId}")]
        public IActionResult GetAverageRating(int productId)
        {
            var connection = (NpgsqlConnection)_context.Database.GetDbConnection();

            try
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT get_average_rating(@productId)", connection))
                {
                    cmd.Parameters.AddWithValue("productId", productId);

                    var result = cmd.ExecuteScalar();

                    if (result == DBNull.Value)
                    {
                        return Ok(0);
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при вызове функции: {ex.Message}");
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