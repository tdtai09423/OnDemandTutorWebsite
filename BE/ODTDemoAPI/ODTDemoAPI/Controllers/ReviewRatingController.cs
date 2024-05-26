using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewRatingController : ControllerBase
    {
        private OnDemandTutorContext _context;
        // private readonly ILogger<ReviewRatingController> _logger;
        public ReviewRatingController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("getReviews/{tutorId}")]
        public async Task<ActionResult<IEnumerable<ReviewRating>>> GetAllReviewsByTutorId(int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }
                else
                {
                    var reviewList = await _context.ReviewRatings
                                                   .Where(r => r.TutorId == tutorId)
                                                   .ToListAsync();
                    if (reviewList == null || reviewList.Count == 0)
                    {
                        return Ok("This tutor has no reviews yet.");
                    }
                    return Ok(reviewList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //xem average rating
        [HttpGet("getAverageRating/{tutorId}")]
        public async Task<ActionResult<double>> GetAverageRating(int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("No tutors can be found with this ID. Please try again.");
                }

                var ratings = await _context.ReviewRatings
                                                  .Where(r => r.TutorId == tutorId && r.Rating.HasValue)
                                                  .Select(r => r.Rating.Value)
                                                  .ToListAsync();
                if (ratings == null || ratings.Count == 0)
                {
                    return NotFound("This tutor has not ratings yet.");
                }

                double averageeRating = (double) ratings.Average();
                return Ok(averageeRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //xem review & rating list cua learner
        //code here
        //xem review & rating cua learner danh cho moi tutor
        //code here
        //them 1 review rating moi
        [HttpPost]
        public async Task<ActionResult> CreateNewReviewRating(ReviewRating reviewRating)
        {
            try
            {
                _context.ReviewRatings.Add(reviewRating);
                await _context.SaveChangesAsync();
                return Ok("Add successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
