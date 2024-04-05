using Ideas.Models;
using Microsoft.AspNetCore.Mvc;


namespace Ideas.Controllers.Reviws
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ProductReviewContext _context;

        public ReviewsController(ProductReviewContext context)
        {
            _context = context;
        }

        [HttpPost("AddReview")]
        public ActionResult<Review> AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();

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
    }
}