namespace Lab2Recommendations;

public class Authenticator :  IAuthenticator
{
    private readonly IAccountRepository _accounts;
    private int _currentId = 0; // 0 means not logged in

    public Authenticator(IAccountRepository accounts)
    {
        _accounts = accounts; 
    }
    public int CurrentId => _currentId;
    public bool IsLoggedIn => _currentId != 0;
    public void LoginById(int id) 
    { 
        var m  = _accounts.GetById(id); 
        _currentId = (m == null?0 : m._Account); 
    }
    public void LoginByName(string name) 
    { 
        var m = _accounts.GetOrAdd(name); 
        _currentId = m._Account; 
    }

    public void Logout()
    {
        _currentId = 0; 
    }
}
