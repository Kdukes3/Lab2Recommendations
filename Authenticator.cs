namespace Lab2Recommendations;

public class Authenticator : IAuthenticator
{
    private readonly IAccountRepository _accounts;
    private int _currentId = 0; // 0 means not logged in
    public string message;

    public Authenticator(IAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public int CurrentId => _currentId;
    public bool IsLoggedIn => _currentId != 0;

    public bool LoginById(int id)
    {
        var m = _accounts.GetById(id);
        if (m == null)
        {
            message = $"Login failed: No account found with id {id}";
            return false;
        }

        _currentId = (m == null ? 0 : m._Account);
        message = $"{m.Name}, you are logged in! (ID: {m._Account}";
        return true;
        //stay logged in if not found
    }

    public bool LoginByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            message = "Login failed: Name is required";
            return false;
        }

        var m = _accounts.GetOrAdd(name);
        if (m == null)
        {
            message = $"Login failed: No account found with the name {name}";
            return false;
        }

        _currentId = m._Account;
        message = $"{m.Name} you are logged in! (ID:  {m._Account}";
        return true;
    }

    public void Logout()
    {
        _currentId = 0;
    }
}