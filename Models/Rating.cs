namespace Lab2Recommendations;

public class Rating
{
    public int memberId { get; set; }
    public int isbn { get; set; }
    public int value { get; set; }

    public Rating(int memberId, int isbn, int value)
    {
        this.memberId = memberId;
        this.isbn = isbn;
        this.value = value;
    }
}