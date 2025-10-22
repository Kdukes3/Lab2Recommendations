namespace Lab2Recommendations;

public interface IAuthenticator
{
    int CurrentId { get; }
    bool IsLoggedIn { get; }
    void LoginById(int id);
    void LoginByName(string name);
    void Logout();
}