namespace Lab2Recommendations.Interfaces;

public interface IMenuAction
{
    string Key { get; }
    string Label { get; }
    void Execute();
}