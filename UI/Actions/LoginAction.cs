using Lab2Recommendations.Interfaces;

namespace Lab2Recommendations.UI.Actions;

public class LoginAction : IMenuAction
{
    private readonly IAuthenticator _auth;
    private readonly Action _onLoginSuccess;

    public LoginAction(IAuthenticator auth, Action onLoginSuccess)
    {
        _auth = auth;
        _onLoginSuccess = onLoginSuccess;
    }

    public string Key => "3";
    public string Label => "Login";

    public void Execute()
    {
        Console.Write("Enter member ID or name: ");
        var raw = (Console.ReadLine() ?? "").Trim();

        bool ok = int.TryParse(raw, out var id)
            ? _auth.LoginById(id)
            : _auth.LoginByName(raw);

        Console.WriteLine(_auth.Message + "\n");
        if (ok) _onLoginSuccess();
    }
}