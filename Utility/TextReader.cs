using Lab2Recommendations.Interfaces;
using Lab2Recommendations.Models;

namespace Lab2Recommendations.Utility;
public class TextReader : IDataReader
{
    public void Seed(IBookRepository books, IAccountRepository accounts, IRatingRepository ratings, string booksPath, string ratingsPath)
    {
        LoadBooks(books, booksPath);
        LoadRatings(books, accounts, ratings, ratingsPath);
    }


    private void LoadBooks(IBookRepository books, string path)
    {
        string[] lines = File.ReadAllLines(path); 
        int next = 1;
        for(int i = 0; i < lines.Length; i++)
        {
            string raw = lines[i]; 
            if(string.IsNullOrWhiteSpace(raw)) 
                continue;
            
            int first = raw.IndexOf(','); 
            int last = raw.LastIndexOf(',');
            if(first < 0 || last <= first) 
                continue;
            
            string author = raw.Substring(0, first).Trim();
            string title = raw.Substring(first + 1, last - first - 1).Trim();
            string year = raw.Substring(last + 1).Trim();
            
            books.Add(new Book(next++, title, author, year));
        }
    }


    private void LoadRatings(IBookRepository books, IAccountRepository accounts, IRatingRepository ratings, string path)
    {
        string[] lines = File.ReadAllLines(path);
        for(int i = 0; i < lines.Length; i++)
        {
            string name = (lines[i]??"").Trim(); 
            if(string.IsNullOrWhiteSpace(name)) 
                continue;
            
            if(i + 1 >= lines.Length) break; 
            string row = lines[++i];
            
            name = MakeUnique(name, accounts);
            var m = accounts.GetOrAdd(name);
            
            string[] parts = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < parts.Length && idx < books.Count; idx++)
            {
                if (int.TryParse(parts[idx], out int v) && v != 0)
                {
                    ratings.Set(m._Account, idx + 1, v);
                }
            }
        }
    }


    private string MakeUnique(string name, IAccountRepository accounts)
    {
        int copy = 1;
        string user = name ;
        while(true)
        {
            bool exists=false;
            var all=accounts.All();
            for(int i=0; i < all.Count; i++)
                if (string.Equals(all[i].Name, user, StringComparison.OrdinalIgnoreCase))
                {
                    exists = true; 
                    break;
                }
            if(!exists) 
                return user; 
            copy++; 
            user = name + " #" + copy.ToString();
        }
    }
}