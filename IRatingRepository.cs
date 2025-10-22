namespace Lab2Recommendations;

public interface IRatingRepository
{
    int Get(int memberId, int isbn);
    void Set(int memberId, int isbn, int value);
    List<Rating> GetAll();
}