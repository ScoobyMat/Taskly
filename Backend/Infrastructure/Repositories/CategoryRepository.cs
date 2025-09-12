using Domain.Interfaces;

public class CategoryRepository : ICategoryRepository
{
    private static readonly List<string> _categories = new()
        {
            "Praca",
            "Nauka/Rozwój",
            "Dom",
            "Zdrowie",
            "Finanse",
            "Projekty osobiste",
            "Rodzina/Przyjaciele",
            "Zakupy",
            "Inne",
        };

    public Task<IEnumerable<string>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<string>>(_categories.OrderBy(c => c).ToList());
    }
}
