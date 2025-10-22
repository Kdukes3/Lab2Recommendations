namespace Lab2Recommendations;

public class RatingService :  IRatingService
{
    private readonly IRatingRepository _repository;

    public RatingService(IRatingRepository repository)
    {
        _repository = repository;
    }

    public int GetRating(int memberId, int bookIsbn)
    {
        return _repository.Get(memberId, bookIsbn);
    }

    public void SetRating(int memberId, int bookIsbn, int value)
    {
        // Basic validation logic can live here instead of the UI
        if (value < -5 || value > 5)
        {
            Console.WriteLine("Rating must be between -5 and 5.");
            return;
        }

        _repository.Set(memberId, bookIsbn, value);
    }

    public List<Rating> GetRatingsForMember(int memberId)
    {
        var result = new List<Rating>();
        foreach (Rating r in _repository.GetAll())
        {
            if (r.memberId == memberId)
                result.Add(r);
        }
        return result;
    }
}