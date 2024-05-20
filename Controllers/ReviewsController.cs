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

        [HttpPost("AddFirstReview")]
        public ActionResult<Review> AddFirstReview(Review review)
        {
            review.ReadyFor2 = DateTime.UtcNow.AddDays(7);
            _context.Reviews.Add(review);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPost("UpdateReview")]
        public ActionResult<Review> UpdateReview(Review review)
        {
            var existingReview = _context.Reviews.FirstOrDefault(r => r.ProductId == review.ProductId && r.PhoneNumber == review.PhoneNumber);
            if (existingReview == null)
            {
                return NotFound("Review not found.");
            }

            if (DateTime.Now >= existingReview.ReadyFor2 && existingReview.Points2 == null && existingReview.Ans2 == null)
            {
                existingReview.Points2 = review.Points2;
                existingReview.Ans2 = review.Ans2;
                existingReview.ReadyFor3 = DateTime.UtcNow.AddDays(7);
            }
            else if (DateTime.Now >= existingReview.ReadyFor3 && existingReview.Points3 == null && existingReview.Ans3 == null)
            {
                existingReview.Points3 = review.Points3;
                existingReview.Ans3 = review.Ans3;
            }
            else
            {
                return BadRequest("Not available to change.");
            }

            _context.SaveChanges();

            return Ok("Succesfuly changed.");
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