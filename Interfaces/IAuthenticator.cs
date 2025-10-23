namespace Lab2Recommendations.Interfaces;

public interface IAuthenticator
{
    int CurrentId { get; }
    bool IsLoggedIn { get; }
    string Message { get; }
    bool LoginById(int id);
    bool LoginByName(string name);
    void Logout();
}