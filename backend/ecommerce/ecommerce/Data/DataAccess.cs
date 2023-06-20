using ecommerce.Models;
using ECommerce.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ecommerce.Data
{
  public class DataAccess : IDataAccess
  {
    private readonly IConfiguration configuration;
    private readonly string dbConnection;
    public DataAccess(IConfiguration configuration)
    {
      this.configuration = configuration;
      dbConnection = this.configuration.GetConnectionString("DB");
    }

    public Offer GetOffer(int id)
    {
      var offer = new Offer();
      using(MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };

        string query = "select * from Offers where OfferId =" + id + ";";
        command.CommandText = query;

        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          offer.Id = (int)reader["OfferId"];
          offer.Title = (string)reader["Title"];
          offer.Discount = (int)reader["Discount"];
        }
      }
      return offer;
    }

    public List<ProductCategory> GetProductCategories()
    {
      var productCategories = new List<ProductCategory>();
      using(MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };
        string query = "Select * from productcategories;";
        command.CommandText = query;
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

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

      return productCategories;
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

    public List<Product> GetProducts(string category, string subCategory, int count)
    {
      var products = new List<Product>();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };

        string query = "SELECT * FROM Products WHERE CategoryId = (SELECT CategoryId FROM ProductCategories WHERE Category = @c AND SubCategory = @s) ORDER BY RAND() LIMIT " + count + ";";


        command.CommandText = query;

        command.Parameters.AddWithValue("@c", category);
        command.Parameters.AddWithValue("@s", subCategory);

        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          var product = new Product()
          {
            Id = (int)reader["ProductId"],
            Title = (string)reader["Title"],
             Description = (string)reader["Description"],
            Price = Convert.ToDouble(reader["Price"]),
            Quantity = (int)reader["Quantity"],
            ImageName = (string)reader["ImageName"],

          };

          var categoryId = (int)reader["CategoryId"];
          product.ProductCategory= GetProductCategory(categoryId);

          var offerId = (int)reader["OfferId"];
          product.Offer = GetOffer(offerId);

          products.Add(product);  
        }
      }
      return products;
    }

    public Product GetProduct(int id)
    {
      var product = new Product();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };

        string query = "select * from products where ProductId =" + id + ";";
        command.CommandText = query;

        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          product.Id = (int)reader["ProductId"];
            product.Title = (string)reader["Title"];
          product.Description = (string)reader["Description"];
          product.Price = Convert.ToDouble(reader["Price"]);
          product.Quantity = (int)reader["Quantity"];
          product.ImageName = (string)reader["ImageName"];

          var categoryId = (int)reader["CategoryId"];
          product.ProductCategory = GetProductCategory(categoryId);

          var offerId = (int)reader["OfferId"];
          product.Offer = GetOffer(offerId);
        }
      }
      return product;
    }

    public bool InsertUser(User user)
    {
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        connection.Open();

        string query = "SELECT COUNT(*) FROM Users WHERE Email=@em;";
        command.CommandText = query;
        command.Parameters.Add("@em", MySqlDbType.VarChar).Value = user.Email;
        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count > 0)
        {
          connection.Close();
          return false;
        }

        query = "INSERT INTO Users (FirstName, LastName, Address, Mobile, Email, Password, CreatedAt, ModifiedAt) VALUES (@fn, @ln, @add, @mb, @em, @pwd, @cat, @mat);";

        command.CommandText = query;
        command.Parameters.Clear();
        command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = user.FirstName;
        command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = user.LastName;
        command.Parameters.Add("@add", MySqlDbType.VarChar).Value = user.Address;
        command.Parameters.Add("@mb", MySqlDbType.VarChar).Value = user.Mobile;
        command.Parameters.Add("@em", MySqlDbType.VarChar).Value = user.Email;
        command.Parameters.Add("@pwd", MySqlDbType.VarChar).Value = user.Password;
        command.Parameters.Add("@cat", MySqlDbType.VarChar).Value = user.CreatedAt;
        command.Parameters.Add("@mat", MySqlDbType.VarChar).Value = user.ModifiedAt;

        command.ExecuteNonQuery();
      }
      return true;
    }

    public string IsUserPresent(string email, string password)
    {
      User user = new User();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        connection.Open();
        string query = "SELECT COUNT(*) FROM Users WHERE Email=@Email AND Password=@Password;";
        command.CommandText = query;
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Password", password);
        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
          connection.Close();
          return "";
        }

        query = "SELECT * FROM Users WHERE Email=@Email AND Password=@Password;";
        command.CommandText = query;

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          user.Id = Convert.ToInt32(reader["UserId"]);
          user.FirstName = reader["FirstName"].ToString();
          user.LastName = reader["LastName"].ToString();
          user.Email = reader["Email"].ToString();
          user.Address = reader["Address"].ToString();
          user.Mobile = reader["Mobile"].ToString();
          user.Password = reader["Password"].ToString();
          user.CreatedAt = reader["CreatedAt"].ToString();
          user.ModifiedAt = reader["ModifiedAt"].ToString();
        }

        string key = "b9d786d25e4b3a532d6e214c6c8a2bcf";
        string duration = "60";
        var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha256);

        // the data in the tokens will be in claims
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("address", user.Address),
            new Claim("mobile", user.Mobile),
            new Claim("email", user.Email),
            new Claim("createdAt", user.CreatedAt),
            new Claim("modifiedAt", user.ModifiedAt)
        };

        var jwtToken = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(duration)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
      }
      return "";
    }

  }
}
