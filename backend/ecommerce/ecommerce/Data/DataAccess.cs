using ecommerce.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;



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
  }
}
