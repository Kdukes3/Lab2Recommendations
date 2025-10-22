namespace Lab2Recommendations;

public interface IAuthenticator
{
    int CurrentId { get; }
    bool IsLoggedIn { get; }
    bool LoginById(int id);
    bool LoginByName(string name);
    void Logout();
}