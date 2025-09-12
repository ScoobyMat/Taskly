using System.Collections;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<string>> GetAllAsync();
    }
}
