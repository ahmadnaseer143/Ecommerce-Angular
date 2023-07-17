using ecommerce.Data.Interfaces;
using ecommerce.Models;
using MySql.Data.MySqlClient;

namespace ecommerce.Data
{
  public class CategoryDataAccess : ICategoryDataAccess
  {
    private readonly IConfiguration configuration;
    private readonly string dbConnection;
    public CategoryDataAccess(IConfiguration configuration)
    {
      this.configuration = configuration;
      dbConnection = this.configuration.GetConnectionString("DB");
    }

    public async Task<List<ProductCategory>> GetProductCategories()
    {
      var productCategories = new List<ProductCategory>();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };
        string query = "Select * from productcategories;";
        command.CommandText = query;
        await connection.OpenAsync();
        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
        {
          while (reader.Read())
          {
            var category = new ProductCategory()
            {
              Id = (int)reader["CategoryId"],
              Category = (string)reader["Category"],
              SubCategory = (string)reader["SubCategory"]
            };
            productCategories.Add(category);
          }
        }
      }

      return productCategories;
    }

    public async Task<bool> InsertProductCategory(ProductCategory productCategory)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand
        {
          Connection = connection
        };

        string query = "INSERT INTO productcategories (Category, SubCategory) VALUES (@Category, @SubCategory)";
        command.CommandText = query;
        command.Parameters.AddWithValue("@Category", productCategory.Category);
        command.Parameters.AddWithValue("@SubCategory", productCategory.SubCategory);

        try
        {
          await connection.OpenAsync();
          int rowsAffected = await command.ExecuteNonQueryAsync();
          return rowsAffected > 0;
        }
        catch (Exception ex)
        {
          // Handling any potential exceptions
          Console.WriteLine($"Error inserting product category: {ex.Message}");
          return false;
        }
      }
    }

    public async Task<bool> UpdateCategory(ProductCategory category)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand
        {
          Connection = connection
        };

        string query = "UPDATE productcategories SET Category = @Category, SubCategory = @SubCategory WHERE CategoryId = @CategoryId";
        command.CommandText = query;
        command.Parameters.AddWithValue("@CategoryId", category.Id);
        command.Parameters.AddWithValue("@Category", category.Category);
        command.Parameters.AddWithValue("@SubCategory", category.SubCategory);

        try
        {
          await connection.OpenAsync();
          int rowsAffected = await command.ExecuteNonQueryAsync();
          return rowsAffected > 0;
        }
        catch (Exception ex)
        {
          // Handling any potential exceptions
          Console.WriteLine($"Error updating category: {ex.Message}");
          return false;
        }
      }
    }


    public async Task<bool> DeleteProductCategory(int id)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand
        {
          Connection = connection
        };

        // Delete products associated with the category
        string deleteProductsQuery = "DELETE FROM products WHERE CategoryId = @Id";
        command.CommandText = deleteProductsQuery;
        command.Parameters.AddWithValue("@Id", id);

        try
        {
          await connection.OpenAsync();
          await command.ExecuteNonQueryAsync();

          // Delete the product category
          string deleteCategoryQuery = "DELETE FROM productcategories WHERE CategoryId = @Id";
          command.CommandText = deleteCategoryQuery;
          int rowsAffected = await command.ExecuteNonQueryAsync();

          return rowsAffected > 0;
        }
        catch (Exception ex)
        {
          // Handling any potential exceptions
          Console.WriteLine($"Error deleting product category: {ex.Message}");
          return false;
        }
      }
    }


    public ProductCategory GetProductCategory(int id)
    {
      var productCategory = new ProductCategory();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };

        string query = "select * from productCategories where CategoryId =" + id + ";";
        command.CommandText = query;

        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          productCategory.Id = (int)reader["CategoryId"];
          productCategory.Category = (string)reader["Category"];
          productCategory.SubCategory = (string)reader["SubCategory"];
        }
      }
      return productCategory;
    }
  }
}
