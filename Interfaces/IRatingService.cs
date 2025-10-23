using Lab2Recommendations.Models;

namespace Lab2Recommendations.Interfaces;

public interface IRatingService
{
    // Simple validation wrapper around IRatingRepository
    void SetRating(int memberId, int isbn, int value);
    int GetRating(int memberId, int isbn);
    List<Rating> GetRatingsForMember(int memberId);
}