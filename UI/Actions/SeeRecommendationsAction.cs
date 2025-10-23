using Lab2Recommendations.Interfaces;
using Lab2Recommendations.Models;

namespace Lab2Recommendations.UI.Actions;

public class SeeRecommendationsAction : IMenuAction
{
    private readonly IRecommendationEngine _recs;
    private readonly Func<int> _memberId;

    public SeeRecommendationsAction(IRecommendationEngine recs, Func<int> memberId)
    {
        _recs = recs; _memberId = memberId;
    }

    public string Key => "5";
    public string Label => "See recommendations";

    public void Execute()
    {
        var (neighbor, reallyLiked, liked) = _recs.RecommendFromNearest(_memberId());
        if (neighbor == null)
        {
            Console.WriteLine("No neighbor found yet. Add more members/ratings.\n");
            return;
        }
        
        Console.WriteLine($"Nearest neighbor: {neighbor.Name} (ID {neighbor._Account})");
        Console.WriteLine("They really liked (5):");
        if (reallyLiked.Count == 0) Console.WriteLine("- none -");
        foreach (var b in reallyLiked) Console.WriteLine($"- {b._Isbn}: {b.Title}");

        Console.WriteLine("They liked (1 or 3):");
        if (liked.Count == 0) Console.WriteLine("- none -");
        foreach (var b in liked) Console.WriteLine($"- {b._Isbn}: {b.Title}");
        Console.WriteLine("");
    }
}