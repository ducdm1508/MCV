using Microsoft.Data.SqlClient;
using UnitOfWork.Data;
using UnitOfWork.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Repositories
{
    public class CategoryRepository : IGenericRepository<Category>
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = new List<Category>();

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT Id, CategoryName FROM Category";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            CategoryName = reader["CategoryName"].ToString()
                        });
                    }
                }
            }

            return categories;
        }

        public async Task<Category?> GetById(int id)
        {
            Category? category = null;

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT Id, CategoryName FROM Category WHERE Id = @Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            category = new Category
                            {
                                Id = reader.GetInt32(0),
                                CategoryName = reader["CategoryName"].ToString()
                            };
                        }
                    }
                }
            }

            return category;
        }

        public async Task Add(Category category)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var sql = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(Category category)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var sql = "UPDATE Category SET CategoryName = @CategoryName WHERE Id = @Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Id", category.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task Delete(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var sql = "DELETE FROM Category WHERE Id = @Id";

                using (var command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
