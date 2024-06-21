using ODTDemoAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODTDemoAPI.Services
{
    public interface ILearnerFavouriteService
    {
        Task<List<LearnerFavourite>> GetAllLearnerFavouritesAsync();
        Task<List<Tutor>> GetLearnerFavouriteTutorsAsync(int learnerId);
        Task AddLearnerFavouriteAsync(LearnerFavourite learnerFavourite);
        Task RemoveLearnerFavouriteAsync(int learnerId, int tutorId);
    }
}
