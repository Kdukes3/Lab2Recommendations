namespace Lab2Recommendations;

public class Authenticator :  IAuthenticator
{
	private readonly IAccountRepository _accounts;
	private int _currentId = -1; // default no one logged in

	public Authenticator(IAccountRepository accounts)
	{
		_accounts = accounts ?? throw new System.ArgumentNullException(nameof(accounts));
	}

	// -1 if no one is logged in
	public int CurrentId => _currentId;

	public bool IsLoggedIn => _currentId != -1; // bool for login status

	public void LoginById(int id)
	{
		var m = _accounts.GetById(id);
		if (m != null)
			_currentId = m._Account;
		// stay logged out if not found
	}

	public void LoginByName(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) return;
		var m = _accounts.GetOrAdd(name.Trim());
		if (m != null) _currentId = m._Account;
	}

	public void Logout()
	{
		_currentId = -1;
	}
}
