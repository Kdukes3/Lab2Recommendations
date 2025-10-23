using Lab2Recommendations.Models;

namespace Lab2Recommendations.Interfaces;

public interface IRecommendationEngine
{
    // Finds the single nearest neighbor and returns two lists of books
    // they rated 5 (really liked) and {1,3} (liked) that the member hasn't rated yet.
    (Member neighbor, List<Book> reallyLiked, List<Book> liked) RecommendFromNearest(int memberId);
}