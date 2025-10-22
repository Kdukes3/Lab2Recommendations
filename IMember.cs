namespace Lab2Recommendations;

public interface IMember
{
    string Name { get; set; }
    int Account { get; set; }
    public void Member(int account,  string name);
}