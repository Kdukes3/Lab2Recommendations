namespace Lab2Recommendations;

public interface IRatingService
{
    // Simple validation wrapper around IRatingRepository
    void Rate(int memberId, int isbn, int value);
    int Get(int memberId, int isbn);
    List<(int isbn,int value)> ForMember(int memberId);
}