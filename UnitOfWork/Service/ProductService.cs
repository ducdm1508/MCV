using UnitOfWork.DTOs;
using UnitOfWork.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _unitOfWork.Products.GetAll();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price
            });
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _unitOfWork.Products.GetById(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price
            };
        }

        public async Task AddProduct(ProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                Price = dto.Price
            };

            await _unitOfWork.Products.Add(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateProduct(ProductDto dto)
        {
            var product = new Product
            {
                Id = dto.Id,
                ProductName = dto.ProductName,
                Price = dto.Price
            };

            await _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProduct(int id)
        {
            await _unitOfWork.Products.Delete(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
