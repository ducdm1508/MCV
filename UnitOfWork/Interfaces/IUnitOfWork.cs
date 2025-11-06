using UnitOfWork.Repositories;

namespace UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ProductRepository Products { get; }
        CategoryRepository Categories { get; }
        ProductCategoryRepository ProductCategories { get; }

        Task SaveAsync();
    }
}
