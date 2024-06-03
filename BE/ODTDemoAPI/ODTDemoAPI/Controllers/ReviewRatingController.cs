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

                var averageRating = await _context.ReviewRatings
                                                  .Where(r => r.TutorId == tutorId && r.Rating.HasValue)
                                                  .AverageAsync(r => r.Rating);
                if (averageRating == null)
                {
                    return NotFound("This tutor has not ratings yet.");
                }
                return Ok(averageRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //getAverageRating theo phương pháp Bayesian: tính trung bình cộng và làm mịn kết quả. (tutor có ít đánh giá ko bị đánh quá cao hoặc quá thấp so với thực tế
        [HttpGet("average-rating-bayesian/{tutorId}")]
        public async Task<ActionResult<double>> GetAverageRatingBaye(int tutorId)
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
                if(ratings == null || ratings.Count == 0)
                {
                    return NotFound("This tutor has no ratings yet.");
                }

                var globalAverage = await _context.ReviewRatings
                                                   .Where(r => r.TutorId == tutorId)
                                                   .AverageAsync(r => r.Rating);
                int minRatings = 5;

                double? bayesianAverage = (ratings.Sum() + minRatings * globalAverage) / (ratings.Count() + minRatings);

                return Ok(bayesianAverage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //getAverageRating theo phuong pháp Wilson Score Confident Interval: tính toán độ tin cậy của kết quả đánh giá
        [HttpGet("average-rating-wilson/{tutorId}")]
        public async Task<ActionResult<double>> GetAverageRatingWilson(int tutorId)
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
                    return NotFound("This tutor has no ratings yet.");
                }

                int totalRatings = ratings.Count();
                int positiveRatings = ratings.Count(r => r >= 3);

                double z = 1.96; //95% confidence
                double phat =  (double) positiveRatings / totalRatings;

                double lowerBound = (phat + z * z / (2 * totalRatings) - z * Math.Sqrt((phat * (1 - phat) + z * z / (4 * totalRatings)) / totalRatings)) / (1 + z * z / totalRatings);

                return Ok(lowerBound);
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
