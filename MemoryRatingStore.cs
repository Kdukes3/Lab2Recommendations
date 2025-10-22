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
        throw new Exception("Book Not found");
    }

    public void Set(int memberId, int isbn, int value)
    {
        Rate.Add(new Rating(isbn, memberId, value));
    }

    public List<Rating> GetAll()
    {
        return Rate;
    }

    // public List<(int isbn, int value)> ForMember(int memberId)
    // {
    //     List<(int isbn, int value)> RatingList = new List<(int isbn, int value)>();
    //     foreach (var rate in Rate)
    //     {
    //         int rateIsbn = rate.isbn;
    //         int rateValue = rate.value;
    //         RatingList.Add(rateIsbn, rateValue);
    //     }
    // }
}
