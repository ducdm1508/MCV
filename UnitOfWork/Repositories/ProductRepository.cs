using Microsoft.Data.SqlClient;  
using UnitOfWork.Data;
using UnitOfWork.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Repositories
{
    public class ProductRepository : IGenericRepository<Product>
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

 
        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = new List<Product>();

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, ProductName, Price FROM Product", (SqlConnection)connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            Price = reader.GetDecimal(2)
                        });
                    }
                }
            }

            return products;
        }

        public async Task<Product?> GetById(int id)
        {
            Product? product = null;

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, ProductName, Price FROM Product WHERE Id = @Id", (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            };
                        }
                    }
                }
            }

            return product;
        }

        public async Task Add(Product product)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "INSERT INTO Product (ProductName, Price) VALUES (@ProductName, @Price)",
                    (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(Product product)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "UPDATE Product SET ProductName = @ProductName, Price = @Price WHERE Id = @Id",
                    (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("DELETE FROM Product WHERE Id = @Id", (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
