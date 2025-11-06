using UnitOfWork.Data;
using UnitOfWork.Interfaces;
using UnitOfWork.Repositories;

namespace UnitOfWork.UnitOfWork
{
    public class AppUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ProductRepository Products { get; }
        public CategoryRepository Categories { get; }
        public ProductCategoryRepository ProductCategories { get; }

        public AppUnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
        }

        public async Task SaveAsync()
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}
