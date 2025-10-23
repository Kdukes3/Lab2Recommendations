namespace Lab2Recommendations;

public class AccountRepository: IAccountRepository
{
    private readonly List<Member> _members = new List<Member>();
    private int _nextId = 1;
    public int Count => _members.Count;
    
    public Member GetOrAdd(string name)
    {
        for (int i=0; i <_members.Count;i++)
            if (string.Equals(_members[i].Name, name, System.StringComparison.OrdinalIgnoreCase)) 
                return _members[i];
        var m = new Member(_nextId++, name); 
        _members.Add(m); 
        return m;
    }

    public Member GetById(int _Account)
    {
        for (int i=0;i < _members.Count; i++) 
            if (_members[i]._Account == _Account) 
                return _members[i]; 
        return null;
    }

    public List<Member> All() => _members;
}