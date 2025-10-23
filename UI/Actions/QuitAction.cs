namespace Lab2Recommendations;

public class QuitAction : IMenuAction
{
    private readonly Action _onQuit;
    public QuitAction(Action onQuit) => _onQuit = onQuit;

    public string Key => "4";
    public string Label => "Quit";

    public void Execute() => _onQuit();
}