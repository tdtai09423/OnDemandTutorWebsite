﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;
using System.Drawing.Printing;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FilterController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public FilterController(OnDemandTutorContext context)
        {
            _context = context;
        }
        //filter nationality
        [HttpGet("get-by-nationality")]
        public async Task<IActionResult> GetTutorByNationality([FromQuery] string? nationality, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(nationality))
            {
                return NoContent();
            }

            var tutors = await _context.Tutors.Where(t => t.Nationality.Contains(nationality)).ToListAsync();
            var totalCount = tutors.Count();
            var pagedTutor = tutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = pagedTutor
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }
        //filter major
        [HttpGet("get-by-major")]
        public async Task<IActionResult> GetTutorByMajor([FromQuery] string? major, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(major))
            {
                return NoContent();
            }

            var tutors = await _context.Tutors
                .Include(t => t.Major)
                .Where(t => t.Major != null && t.Major.MajorName.Contains(major))
                .ToListAsync();

            var totalCount = tutors.Count();
            var pagedTutor = tutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = pagedTutor
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }
        //filter range price
        [HttpGet("tutors-by-price-range")]
        public async Task<IActionResult> GetTutorsByPriceRange([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                var totalCount = tutors.Count();
                var pagedTutor = tutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tutors-by-price-range-desc")]
        public async Task<IActionResult> GetTutorsByPriceRangeDesc([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (minPrice == null && maxPrice == null)
            {
                return NoContent();
            }

            try
            {
                var query = _context.Tutors.Include(t => t.Curricula).AsQueryable();

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
                var totalCount = tutors.Count();
                var pagedTutor = tutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tutors-by-price-range-asc")]
        public async Task<IActionResult> GetTutorsByPriceRangeAsc([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                var totalCount = tutors.Count();
                var pagedTutor = tutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //filter rating-tutor tang dan
        [HttpGet("tutors-by-rating-asc")]
        public async Task<IActionResult> GetTutorsByRatingAsc([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

                var totalCount = tutorsWithRatings.Count();
                var pagedTutor = tutorsWithRatings.Skip((page - 1) * pageSize).Take(pageSize).Select(t => t.Tutor).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //filter rating-tutor giam dan
        [HttpGet("tutors-by-rating-desc")]
        public async Task<IActionResult> GetTutorsByRatingDesc([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

                var totalCount = tutorsWithRatings.Count();
                var pagedTutor = tutorsWithRatings.Skip((page - 1) * pageSize).Take(pageSize).Select(t => t.Tutor).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // filter number of rating
        [HttpGet("tutors-by-rating-count-desc")]
        public async Task<IActionResult> GetTutorsByRatingCountDesc([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

                var totalCount = tutorsWithRatingCounts.Count();
                var pagedTutor = tutorsWithRatingCounts.Skip((page - 1) * pageSize).Take(pageSize).Select(t => t.Tutor).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // search tutor name
        [HttpGet("search-tutors-by-name")]
        public async Task<IActionResult> SearchTutorsByName([FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

                var totalCount = filteredTutors.Count();
                var pagedTutor = filteredTutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages += 1;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-available-tutors-by-date")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAvailableTutorsByDate([FromQuery] DateTime date, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var tutors = await _context.Tutors.ToListAsync();

            var availableTutors = new List<Tutor>();

            DateTime startOfDay = date.Date.AddHours(7);
            DateTime endOfDay = date.Date.AddHours(23);

            foreach (var tutor in tutors)
            {
                var sections = await _context.Sections
                                    .Where(s => s.Curriculum!.LearnerOrders.Any(lo => lo.Curriculum!.TutorId == tutor.TutorId) && s.SectionStart.Date == date.Date)
                                    .OrderBy(s => s.SectionStart)
                                    .ToListAsync();

                //nếu tutor không có section nào => tutor rảnh cả ngày => add vào
                if (!sections.Any())
                {
                    availableTutors.Add(tutor);
                    continue;
                }

                DateTime previousEnd = startOfDay;
                bool hasAtLeastOneHourFree = false;

                foreach (var section in sections)
                {
                    if ((section.SectionStart - previousEnd).TotalSeconds >= 60 * 60) //rảnh ít nhất 1 giờ
                    {
                        hasAtLeastOneHourFree = true;
                        break;
                    }

                    previousEnd = section.SectionEnd;
                }

                if (!hasAtLeastOneHourFree && (endOfDay - previousEnd).TotalSeconds >= 60 * 60)
                {
                    hasAtLeastOneHourFree = true;
                }

                if (hasAtLeastOneHourFree)
                {
                    availableTutors.Add(tutor);
                }
            }

            var totalCount = availableTutors.Count();
            var pagedTutor = availableTutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = pagedTutor
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("filter-tutors")]
        public async Task<IActionResult> FilterTutors(
    [FromQuery] string? nationality,
    [FromQuery] string? major,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] decimal? minRating,
    [FromQuery] decimal? maxRating,
    [FromQuery] bool? ratingCountDesc,
    [FromQuery] bool priceRangeAsc = true,
    [FromQuery] bool ratingAsc = true,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                IQueryable<Tutor> query = _context.Tutors
                    .Include(t => t.ReviewRatings)
                    .Include(t => t.Major)
                    .Include(t => t.Curricula)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(nationality))
                {
                    query = query.Where(t => t.Nationality.Contains(nationality));
                }

                if (!string.IsNullOrEmpty(major))
                {
                    query = query.Where(t => t.Major != null && t.Major.MajorName.Contains(major));
                }

                if (minPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection >= minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(t => t.Curricula.Any(c => c.PricePerSection <= maxPrice.Value));
                }

                // Sorting by price
                if (priceRangeAsc)
                {
                    query = query.OrderBy(t => t.Curricula.Min(c => c.PricePerSection));
                }
                else
                {
                    query = query.OrderByDescending(t => t.Curricula.Max(c => c.PricePerSection));
                }

                var tutors = await query.ToListAsync();

                // Calculate Wilson scores
                var tutorWithScores = new List<(Tutor Tutor, double? WilsonScore)>();
                foreach (var tutor in tutors)
                {
                    var score = await CalculateWilsonScoreAsync(tutor.TutorId);
                    tutorWithScores.Add((tutor, score));
                }

                // Filtering by rating
                if (minRating.HasValue || maxRating.HasValue)
                {
                    tutorWithScores = tutorWithScores.Where(t => t.WilsonScore.HasValue).ToList();

                    if (minRating.HasValue)
                    {
                        tutorWithScores = tutorWithScores.Where(t => t.WilsonScore!.Value >= (double)minRating.Value).ToList();
                    }

                    if (maxRating.HasValue)
                    {
                        tutorWithScores = tutorWithScores.Where(t => t.WilsonScore!.Value <= (double)maxRating.Value).ToList();
                    }
                }

                // Sorting by Wilson score
                if (ratingAsc)
                {
                    tutorWithScores = tutorWithScores.OrderBy(t => t.WilsonScore).ToList();
                }
                else
                {
                    tutorWithScores = tutorWithScores.OrderByDescending(t => t.WilsonScore).ToList();
                }

                if (ratingCountDesc.HasValue && ratingCountDesc.Value)
                {
                    tutorWithScores = tutorWithScores.OrderByDescending(t => t.Tutor.ReviewRatings.Count(r => r.Rating.HasValue)).ToList();
                }

                var totalCount = tutorWithScores.Count;
                var pagedTutor = tutorWithScores.Skip((page - 1) * pageSize).Take(pageSize).Select(t => t.Tutor).ToList();

                var response = new PaginatedResponse<Tutor>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = pagedTutor
                };

                int numOfPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<double?> CalculateWilsonScoreAsync(int tutorId)
        {
            var ratings = await _context.ReviewRatings
                                        .Where(r => r.TutorId == tutorId && r.Rating.HasValue)
                                        .Select(r => r.Rating!.Value)
                                        .ToListAsync();

            if (ratings == null || ratings.Count == 0)
            {
                return null;
            }

            int totalRatings = ratings.Count();
            int positiveRatings = ratings.Count(r => r >= 3);

            double z = 1.96; // 95% confidence
            double phat = (double)positiveRatings / totalRatings;

            double lowerBound = (phat + z * z / (2 * totalRatings) - z * Math.Sqrt((phat * (1 - phat) + z * z / (4 * totalRatings)) / totalRatings)) / (1 + z * z / totalRatings);

            return lowerBound;
        }


    }
}
