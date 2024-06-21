using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;

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

        [HttpGet("getReviews/tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<ReviewRating>>> GetAllReviewsOfTutor(int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
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
                                            .Select(r => r.Rating!.Value)
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
        public async Task<ActionResult<double>> GetAverageRatingWilson([FromRoute] int tutorId)
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
                                            .Select(r => r.Rating!.Value)
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
        [HttpGet("getReviews/learner/{learnerId}")]
        public async Task<ActionResult<IEnumerable<ReviewRating>>> GetAllReviewsOfLearner(int learnerId)
        {
            try
            {
                var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
                if (learner == null)
                {
                    return NotFound("Not found learner.");
                }
                else
                {
                    var reviewList = await _context.ReviewRatings
                                                   .Where(r => r.LearnerId == learnerId)
                                                   .ToListAsync();
                    if (reviewList == null || reviewList.Count == 0)
                    {
                        return Ok("This learner has no reviews yet.");
                    }
                    return Ok(reviewList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //xem review & rating cua learner danh cho moi tutor
        [HttpGet("getReview/{learnerId}/{tutorId}")]
        public async Task<IActionResult> GetReview(int tutorId, int learnerId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }

                var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
                if(learner == null)
                {
                    return NotFound("Not found learner.");
                }

                var review = await _context.ReviewRatings.FirstOrDefaultAsync(r => r.TutorId == tutorId && r.LearnerId == learnerId);
                if (review == null)
                {
                    return NotFound("There is no review between this two account.");
                }

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //them 1 review rating moi
        [HttpPost("review-tutor")]
        public async Task<ActionResult> CreateNewReviewRating(ReviewModel model)
        {
            try
            {
                var newReview = new ReviewRating
                {
                    LearnerId = model.LearnerId,
                    TutorId = model.TutorId,
                    Rating = model.Rating,
                    Review = model.Review,
                    ReviewDate = DateTime.Now,
                };
                _context.ReviewRatings.Add(newReview);
                await _context.SaveChangesAsync();

                return Ok(newReview);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //chỉnh sửa 1 review
        [HttpPut("edit-review")]
        public async Task<IActionResult> EditReview(ReviewModel newModel)
        {
            try
            {
                var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == newModel.LearnerId);
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == newModel.TutorId);

                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }

                if (learner == null)
                {
                    return NotFound("Not found learner.");
                }

                var review = await _context.ReviewRatings.FirstOrDefaultAsync(r => r.TutorId == newModel.TutorId && r.LearnerId == newModel.LearnerId);
                if (review == null)
                {
                    return NotFound("There is no review between this two account.");
                }

                review.Review = newModel.Review;
                review.Rating = newModel.Rating;
                review.ReviewDate = DateTime.Now;

                _context.ReviewRatings.Update(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //review của tutor theo rating
        [HttpGet("get-all-tutor-reviews-rating")]
        public async Task<IActionResult> GetAllReviewsOfTutorByRating(int rating, int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);

                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }

                var reviewList = await _context.ReviewRatings.Where(r => r.Rating == rating).ToListAsync();
                if(reviewList == null || reviewList.Count == 0) 
                {
                    return NotFound("No review has this number of rating.");
                }

                return Ok(reviewList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //xem tất cả review và sắp xếp theo ngày (tính năng quản lí của admin)
        [HttpGet("get-all-reviews")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviewList = await _context.ReviewRatings.OrderByDescending(r => r.ReviewDate).ToListAsync();

                return Ok(reviewList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-review")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteReview(int learnerId, int tutorId)
        {
            try
            {
                var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);

                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }

                if (learner == null)
                {
                    return NotFound("Not found learner.");
                }

                var review = await _context.ReviewRatings.FirstOrDefaultAsync(r => r.TutorId == tutorId && r.LearnerId == learnerId);
                if (review == null)
                {
                    return NotFound("There is no review between this two account.");
                }

                _context.ReviewRatings.Remove(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
