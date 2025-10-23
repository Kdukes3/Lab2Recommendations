namespace Lab2Recommendations;

public interface IMenuAction
{
    string Key { get; }
    string Label { get; }
    void Execute();
}