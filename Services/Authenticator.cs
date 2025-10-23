using Lab2Recommendations.Interfaces;
using Lab2Recommendations.Models;

namespace Lab2Recommendations.Services;

public class Authenticator : IAuthenticator
{
    private readonly IAccountRepository _accounts;
    private int _currentId = 0; // 0 means not logged in
    private string _message = "";

    public Authenticator(IAccountRepository accounts)
    {
        _accounts = accounts;
    }

    public int CurrentId => _currentId;
    public bool IsLoggedIn => _currentId != 0;
    public string Message => _message;

    public bool LoginById(int id)
    {
        var m = _accounts.GetById(id);
        if (m == null)
        {
            _message = $"Login failed: No account found with id {id}";
            return false;
        }
        _currentId = m._Account;
        _message = $"{m.Name}, you are logged in! (ID: {m._Account})";
        return true;
    }

    public bool LoginByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _message = "Login failed: Name is required";
            return false;
        }
        
        var m = _accounts.GetOrAdd(name);
        _currentId = m._Account;
        _message = $"{m.Name}, you are logged in! (ID: {m._Account})";
        return true;
    }

    public void Logout()
    {
        _currentId = 0;
        _message = "You are logged out.";
    }
}