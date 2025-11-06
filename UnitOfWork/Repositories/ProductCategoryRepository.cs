using Microsoft.Data.SqlClient;
using UnitOfWork.Data;
using UnitOfWork.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Repositories
{
    public class ProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAll()
        {
            var list = new List<ProductCategory>();

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                var sql = @"
                    SELECT 
                        pc.ProductId, 
                        pc.CategoryId, 
                        p.ProductName, 
                        p.Price,
                        c.CategoryName
                    FROM ProductCategory pc
                    JOIN Product p ON pc.ProductId = p.Id
                    JOIN Category c ON pc.CategoryId = c.Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new ProductCategory
                        {
                            ProductId = reader.GetInt32(0),
                            CategoryId = reader.GetInt32(1),
                            Product = new Product
                            {
                                Id = reader.GetInt32(0),
                                ProductName = reader.GetString(2),
                                Price = reader.GetDecimal(3)
                            },
                            Category = new Category
                            {
                                Id = reader.GetInt32(1),
                                CategoryName = reader.GetString(4)
                            }
                        });
                    }
                }
            }

            return list;
        }

        public async Task<ProductCategory?> GetById(int id)
        {
            ProductCategory? pc = null;

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                var sql = @"
                    SELECT 
                        pc.ProductId, 
                        pc.CategoryId, 
                        p.ProductName, 
                        p.Price,
                        c.CategoryName
                    FROM ProductCategory pc
                    JOIN Product p ON pc.ProductId = p.Id
                    JOIN Category c ON pc.CategoryId = c.Id
                    WHERE pc.ProductId = @Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            pc = new ProductCategory
                            {
                                ProductId = reader.GetInt32(0),
                                CategoryId = reader.GetInt32(1),
                                Product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    ProductName = reader.GetString(2),
                                    Price = reader.GetDecimal(3)
                                },
                                Category = new Category
                                {
                                    Id = reader.GetInt32(1),
                                    CategoryName = reader.GetString(4)
                                }
                            };
                        }
                    }
                }
            }

            return pc;
        }

        public async Task Add(ProductCategory entity)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                var sql = "INSERT INTO ProductCategory (ProductId, CategoryId) VALUES (@ProductId, @CategoryId)";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@ProductId", entity.ProductId);
                    command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(ProductCategory entity)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                var sql = @"
                    UPDATE ProductCategory
                    SET CategoryId = @CategoryId
                    WHERE ProductId = @ProductId";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@ProductId", entity.ProductId);
                    command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                var sql = "DELETE FROM ProductCategory WHERE ProductId = @Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
