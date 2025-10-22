namespace Lab2Recommendations;

public interface IRatingRepository
{
    int Get(int memberId, int isbn);
    List<Rating> GetAll();
}