using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public Task<IEnumerable<string>> GetAllAsync()
        {
            return _categoryRepo.GetAllAsync();
        }
    }
}
