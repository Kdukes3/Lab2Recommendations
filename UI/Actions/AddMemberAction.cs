namespace Lab2Recommendations;

public class AddMemberAction : IMenuAction
{
    private readonly IAccountRepository _accounts;
    public AddMemberAction(IAccountRepository accounts) => _accounts = accounts;

    public string Key => "1";
    public string Label => "Add a new member";

    public void Execute()
    {
        Console.Write("Enter member name: ");
        var name = (Console.ReadLine() ?? "").Trim();
        var m = _accounts.GetOrAdd(name);
        Console.WriteLine($"Member '{m.Name}' added with ID {m._Account}\n");
    }
}