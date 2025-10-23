using Lab2Recommendations.Models;

namespace Lab2Recommendations.Interfaces;

public interface IAccountRepository
{
    int Count { get; }
    
    Member GetOrAdd(string name);
    Member GetById(int id);
    List<Member> All();
    
}