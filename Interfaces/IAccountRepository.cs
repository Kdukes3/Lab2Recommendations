namespace Lab2Recommendations;

public interface IAccountRepository
{
    int Count { get; }
    
    Member GetOrAdd(string name);
    Member GetById(int id);
    List<Member> All();
    
}