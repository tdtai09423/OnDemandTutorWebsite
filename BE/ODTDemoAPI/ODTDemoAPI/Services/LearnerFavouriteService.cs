
using ODTDemoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTDemoAPI.Services
{
    public class LearnerFavouriteService : ILearnerFavouriteService
    {
        private readonly OnDemandTutorContext _context;

        public LearnerFavouriteService(OnDemandTutorContext context)
        {
            _context = context;
        }

        public async Task<List<LearnerFavourite>> GetAllLearnerFavouritesAsync()
        {
            return await _context.LearnerFavourites
                .Include(lf => lf.Tutor)
                .Include(lf => lf.Learner)
                .ToListAsync();
        }

        public async Task<List<Tutor>> GetLearnerFavouriteTutorsAsync(int learnerId)
        {
            return await _context.LearnerFavourites
                .Where(lf => lf.LearnerId == learnerId)
                .Include(lf => lf.Tutor)
                .Select(lf => lf.Tutor)
                .ToListAsync();
        }

        public async Task AddLearnerFavouriteAsync(LearnerFavourite learnerFavourite)
        {
            _context.LearnerFavourites.Add(learnerFavourite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLearnerFavouriteAsync(int learnerId, int tutorId)
        {
            var learnerFavourite = await _context.LearnerFavourites
                .FirstOrDefaultAsync(lf => lf.LearnerId == learnerId && lf.TutorId == tutorId);
            if (learnerFavourite != null)
            {
                _context.LearnerFavourites.Remove(learnerFavourite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
