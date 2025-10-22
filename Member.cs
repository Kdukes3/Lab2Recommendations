namespace Lab2Recommendations;

public class Member
{
    public Member(int _nextId, string name)
    {
        Name = name;
        _Account = _nextId;
    }
    public string Name;
    public int _Account;
}