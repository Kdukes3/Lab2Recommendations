using Lab2Recommendations.Models;

namespace Lab2Recommendations.Interfaces;

public interface IRatingRepository
{
    int Get(int memberId, int isbn);
    void Set(int memberId, int isbn, int value);
    List<Rating> GetAll();
}