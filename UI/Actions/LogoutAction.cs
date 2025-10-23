namespace Lab2Recommendations;

public class LogoutAction : IMenuAction
{
    private readonly IAuthenticator _auth;
    public LogoutAction(IAuthenticator auth) => _auth = auth;

    public string Key => "6";
    public string Label => "Logout";

    public void Execute()
    {
        _auth.Logout();
        Console.WriteLine("Logged out.\n");
    }
}