using Domain.Entities;

namespace Domain.Interfaces;
public interface ICategoryRepository
{
    Task<IEnumerable<string>> GetAllAsync();
}

