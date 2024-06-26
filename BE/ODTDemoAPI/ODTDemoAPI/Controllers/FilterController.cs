using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilterController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public FilterController(OnDemandTutorContext context)
        {
            _context = context;
        }
        //filter nationality
        [HttpGet("get-by-nationality")]
        public async Task<IActionResult> GetTutorByNationality([FromQuery] string? nationality)
        {
            if (string.IsNullOrWhiteSpace(nationality))
            {
                return NoContent();
            }

            var tutors = await _context.Tutors.Where(t => t.Nationality.Contains(nationality)).ToListAsync();
            return Ok(tutors);
        }
        //filter major
        [HttpGet("get-by-major")]
        public async Task<IActionResult> GetTutorByMajor([FromQuery] string? major)
        {
            if (string.IsNullOrWhiteSpace(major))
            {
                return NoContent();
            }

            var tutors = await _context.Tutors
                .Include(t => t.Major)
                .Where(t => t.Major != null && t.Major.MajorName.Contains(major))
                .ToListAsync();

            return Ok(tutors);
        }
        //filter range price
        [HttpGet("tutors-by-price-range")]
        public async Task<IActionResult> GetTutorsByPriceRange([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            if (minPrice == null && maxPrice == null)
            {
                return NoContent();
            }

            try
            {
                var query = _context.Tutors
                    .Include(t => t.Curricula) // Include Curricula navigation property
                    .AsQueryable();

                if (minPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection >= minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection <= maxPrice.Value));
                }

                var tutors = await query.ToListAsync();
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tutors-by-price-range-desc")]
        public async Task<IActionResult> GetTutorsByPriceRangeDesc(
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice)
        {
            if (minPrice == null && maxPrice == null)
            {
                return NoContent();
            }

            try
            {
                var query = _context.Tutors
                    .Include(t => t.Curricula)
                    .AsQueryable();

                if (minPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection >= minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection <= maxPrice.Value));
                }

                query = query.OrderByDescending(t => t.Curricula.Max(c => c.PricePerSection));

                var tutors = await query.ToListAsync();
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tutors-by-price-range-asc")]
        public async Task<IActionResult> GetTutorsByPriceRangeAsc(
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
        {
            if (minPrice == null && maxPrice == null)
            {
                return NoContent();
            }

            try
            {
                var query = _context.Tutors
                    .Include(t => t.Curricula)
                    .AsQueryable();

                if (minPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection >= minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection <= maxPrice.Value));
                }

                query = query.OrderBy(t => t.Curricula.Min(c => c.PricePerSection));

                var tutors = await query.ToListAsync();
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //filter rating-tutor tang dan
        [HttpGet("tutors-by-rating-asc")]
        public async Task<IActionResult> GetTutorsByRatingAsc()
        {
            try
            {
                var tutorsWithRatings = await _context.Tutors
                    .Select(t => new
                    {
                        Tutor = t,
                        AverageRating = _context.ReviewRatings
                                                .Where(r => r.TutorId == t.TutorId && r.Rating.HasValue)
                                                .Average(r => (double?)r.Rating) ?? 0
                    })
                    .OrderBy(t => t.AverageRating)
                    .ToListAsync();

                return Ok(tutorsWithRatings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //filter rating-tutor giam dan
        [HttpGet("tutors-by-rating-desc")]
        public async Task<IActionResult> GetTutorsByRatingDesc()
        {
            try
            {
                var tutorsWithRatings = await _context.Tutors
                    .Select(t => new
                    {
                        Tutor = t,
                        AverageRating = _context.ReviewRatings
                                                .Where(r => r.TutorId == t.TutorId && r.Rating.HasValue)
                                                .Average(r => (double?)r.Rating) ?? 0
                    })
                    .OrderByDescending(t => t.AverageRating)
                    .ToListAsync();

                return Ok(tutorsWithRatings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // filter number of rating
        [HttpGet("tutors-by-rating-count-desc")]
        public async Task<IActionResult> GetTutorsByRatingCountDesc()
        {
            try
            {
                var tutorsWithRatingCounts = await _context.Tutors
                    .Select(t => new
                    {
                        Tutor = t,
                        RatingCount = _context.ReviewRatings
                                              .Count(r => r.TutorId == t.TutorId && r.Rating.HasValue)
                    })
                    .OrderByDescending(t => t.RatingCount)
                    .ToListAsync();

                return Ok(tutorsWithRatingCounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // search tutor name
        [HttpGet("search-tutors-by-name")]
        public async Task<IActionResult> SearchTutorsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter is required.");
            }

            try
            {

                var tutors = await _context.Tutors
                    .Include(t => t.TutorNavigation)
                    .ToListAsync();

                var filteredTutors = tutors
                    .Where(t => (t.TutorNavigation.FirstName + " " + t.TutorNavigation.LastName)
                    .Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!filteredTutors.Any())
                {
                    return NotFound("No tutors found matching the provided name.");
                }

                return Ok(filteredTutors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
