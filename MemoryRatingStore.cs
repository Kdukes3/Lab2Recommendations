namespace Lab2Recommendations;
public class MemoryRatingStore : IRatingRepository
{
    public List<Rating> Rate = new List<Rating>();
    public int Get(int memberId, int isbn)
    {
        foreach (var rate in Rate)
        {
            if (rate.memberId == memberId && rate.isbn == isbn)
            {
                return rate.value;
            }
        }
        return 0;
    }

    public void Set(int memberId, int isbn, int value)
    {
        for (int i = 0; i < Rate.Count; i++)
        {
            if (Rate[i].memberId == memberId && Rate[i].isbn == isbn)
            {
                Rate[i].value = value;
                return;
            }
        }
        Rate.Add(new Rating(memberId, isbn, value));
    }

    public List<Rating> GetAll()
    {
        return Rate;
    }
}