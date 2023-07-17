using ecommerce.Data.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ecommerce.Data
{
  public class DataAccess : IDataAccess
  {
    private readonly IConfiguration configuration;
    private readonly string dbConnection;
    private readonly string dateformat;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ICategoryDataAccess iCategoryDataAccess;
    private readonly IOfferDataAccess iOfferDataAccess;
    private readonly IUserDataAccess iUserDataAccess;
    public DataAccess(IConfiguration configuration, IWebHostEnvironment hostEnvironment, ICategoryDataAccess iCategoryDataAccess, IOfferDataAccess iOfferDataAccess, IUserDataAccess iUserDataAccess)
    {
      this.configuration = configuration;
      this.iCategoryDataAccess = iCategoryDataAccess;
      this.iOfferDataAccess = iOfferDataAccess;
      this.iUserDataAccess = iUserDataAccess;
      _hostEnvironment = hostEnvironment;
      dbConnection = this.configuration.GetConnectionString("DB");
      dateformat = this.configuration["Constants:DateFormat"];
      this.iOfferDataAccess = iOfferDataAccess;
      this.iUserDataAccess = iUserDataAccess;
    }

    public async Task<List<Product>> GetProducts(string category, string subCategory, int count)
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

        await connection.OpenAsync();

        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
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
          product.ProductCategory= iCategoryDataAccess.GetProductCategory(categoryId);

          var offerId = (int)reader["OfferId"];
          product.Offer =iOfferDataAccess.GetOffer(offerId);

          products.Add(product);  
        }
      }
      return products;
    }

    public async Task<byte[]> GetProductImage(int productId)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection,
          CommandText = "SELECT ImageName FROM Products WHERE ProductId = @productId"
        };

        command.Parameters.AddWithValue("@productId", productId);

        await connection.OpenAsync();

        object result = await command.ExecuteScalarAsync();
        if (result != null && result != DBNull.Value)
        {
          string imagePath = result.ToString();
          byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
          return imageBytes;
        }
      }

      return null;
    }

    public async Task<List<Product>> GetAllProducts()
    {
      var products = new List<Product>();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection,
        };

        string query = "SELECT * FROM Products;";


        command.CommandText = query;

        await connection.OpenAsync();

        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
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
          product.ProductCategory = iCategoryDataAccess.GetProductCategory(categoryId);

          var offerId = (int)reader["OfferId"];
          product.Offer = iOfferDataAccess.GetOffer(offerId);

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

          // Get the image file as base64 string
          if (!string.IsNullOrEmpty(product.ImageName))
          {
            byte[] fileBytes = System.IO.File.ReadAllBytes(product.ImageName);
            string base64String = Convert.ToBase64String(fileBytes);
            product.ImageFile = base64String;
          }

          var categoryId = (int)reader["CategoryId"];
          product.ProductCategory = iCategoryDataAccess.GetProductCategory(categoryId);

          var offerId = (int)reader["OfferId"];
          product.Offer = iOfferDataAccess.GetOffer(offerId);
        }
      }
      return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection,
        };

        string query = "UPDATE products SET Title = @Title, Description = @Description, Price = @Price, Quantity = @Quantity, ImageName = @ImageName, CategoryId = @CategoryId, OfferId = @OfferId WHERE ProductId = @ProductId;";
        command.CommandText = query;

        // Set the parameter values
        command.Parameters.AddWithValue("@Title", product.Title);
        command.Parameters.AddWithValue("@Description", product.Description);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@Quantity", product.Quantity);
        command.Parameters.AddWithValue("@ImageName", product.ImageName);
        command.Parameters.AddWithValue("@CategoryId", product.ProductCategory.Id);
        command.Parameters.AddWithValue("@OfferId", product.Offer.Id);
        command.Parameters.AddWithValue("@ProductId", product.Id);

        await connection.OpenAsync();

        int rowsAffected = command.ExecuteNonQuery();

        // Check if any rows were affected by the update
        if (rowsAffected > 0)
        {
          // Check if there is an updated image file
          if (!string.IsNullOrEmpty(product.ImageFile))
          {
            // Delete the previous image file
            if (!string.IsNullOrEmpty(product.ImageName))
            {
              File.Delete(product.ImageName);
            }

            // Convert the Base64 string to a byte array
            byte[] fileBytes = Convert.FromBase64String(product.ImageFile);

            string fileName = $"{product.Id}.jpg";

            // Define the base directory path and create it if it doesn't exist
            var baseDirectory = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "Resources", "Images");
            Directory.CreateDirectory(baseDirectory);

            // Get the category and subcategory folder paths
            var categoryFolder = Path.Combine(baseDirectory, product.ProductCategory.Category);
            var subcategoryFolder = Path.Combine(categoryFolder, product.ProductCategory.SubCategory);

            // Create category and subcategory folders if they don't exist
            Directory.CreateDirectory(categoryFolder);
            Directory.CreateDirectory(subcategoryFolder);

            // Define the folder path for the productId
            string productIdFolder = Path.Combine(subcategoryFolder, product.Id.ToString());
            Directory.CreateDirectory(productIdFolder);

            // Define the file path
            string filePath = Path.Combine(productIdFolder, fileName);

            // Save the byte array to a file
            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

            // Update the product's ImageName attribute with the file path
            product.ImageName = filePath;
          }

          // Update the image path in the database
          command.CommandText = "UPDATE products SET ImageName = @ImageName WHERE ProductId = @ProductId;";
          command.Parameters.Clear();
          command.Parameters.AddWithValue("@ImageName", product.ImageName);
          command.Parameters.AddWithValue("@ProductId", product.Id);
          command.ExecuteNonQuery();
          return product;
        }
      }

      // If no rows were affected or an error occurred, return null
      return null;
    }

    public async Task InsertReview(Review review)
    {
      using MySqlConnection connection = new(dbConnection);
      MySqlCommand command = new()
      {
        Connection = connection
      };

      string query = "INSERT INTO Reviews (UserId, ProductId, Review, CreatedAt) VALUES (@uid, @pid, @rv, @cat);";
      command.CommandText = query;
      command.Parameters.Add("@uid", MySqlDbType.Int32).Value = review.User.Id;
      command.Parameters.Add("@pid", MySqlDbType.Int32).Value = review.Product.Id;
      command.Parameters.Add("@rv", MySqlDbType.VarChar).Value = review.Value;
      command.Parameters.Add("@cat", MySqlDbType.VarChar).Value = review.CreatedAt;

      await connection.OpenAsync();
      command.ExecuteNonQuery();
    }


    public async Task<List<Review>> GetProductReviews(int productId)
    {
      var reviews = new List<Review>();
      using (MySqlConnection connection = new(dbConnection))
      {
        MySqlCommand command = new()
        {
          Connection = connection
        };

        string query = "SELECT * FROM Reviews WHERE ProductId = @productId;";
        command.CommandText = query;
        command.Parameters.AddWithValue("@productId", productId);

        await connection.OpenAsync();
        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          var review = new Review()
          {
            Id = (int)reader["ReviewId"],
            Value = (string)reader["Review"],
            CreatedAt = (string)reader["CreatedAt"]
          };

          var userId = (int)reader["UserId"];
          review.User = iUserDataAccess.GetUser(userId);

          var id = (int)reader["ProductId"];
          review.Product = GetProduct(productId);

          reviews.Add(review);
        }
      }
      return reviews;
    }

    public async Task<bool> InsertCartItem(int userId, int productId)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        await connection.OpenAsync();
        string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
        command.CommandText = query;
        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
          query = "INSERT INTO Carts (UserId, Ordered, OrderedOn) VALUES (" + userId + ", 'false', '');";
          command.CommandText = query;
          command.ExecuteNonQuery();
        }

        query = "SELECT CartId FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
        command.CommandText = query;
        int cartId = Convert.ToInt32(command.ExecuteScalar());


        query = "INSERT INTO CartItems (CartId, ProductId) VALUES (" + cartId + ", " + productId + ");";
        command.CommandText = query;
        command.ExecuteNonQuery();
        return true;
      }
    }

    public async Task<bool> RemoveCartItem(int userId, int productId)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        await connection.OpenAsync();

        string query = "SELECT CartId FROM Carts WHERE UserId = " + userId + " AND Ordered = 'false';";
        command.CommandText = query;
        int cartId = Convert.ToInt32(command.ExecuteScalar());

        if (cartId != 0)
        {
          query = "DELETE FROM CartItems WHERE CartId = " + cartId + " AND ProductId = " + productId + ";";
          command.CommandText = query;
          int rowsAffected = command.ExecuteNonQuery();

          if (rowsAffected > 0)
          {
            // Item successfully removed from the cart
            return true;
          }
        }

        // Either the cart or the item was not found
        return false;
      }
    }


    async Task<Cart> GetActiveCartOfUser(int userid)
    {
      var cart = new Cart();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) From Carts WHERE UserId=" + userid + " AND Ordered='false';";
        command.CommandText = query;

        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
          return cart;
        }

        query = "SELECT CartId From Carts WHERE UserId=" + userid + " AND Ordered='false';";
        command.CommandText = query;

        int cartid = Convert.ToInt32(command.ExecuteScalar());

        query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            CartItem item = new CartItem()
            {
              Id = (int)reader["CartItemId"],
              Product = GetProduct((int)reader["ProductId"])
            };
            cart.CartItems.Add(item);
          }
        }

        cart.Id = cartid;
        cart.User = iUserDataAccess.GetUser(userid);
        cart.Ordered = false;
        cart.OrderedOn = "";
      }
      return cart;
    }

    async Task<List<Cart>> GetAllPreviousCartsOfUser(int userid)
    {
      var carts = new List<Cart>();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        string query = "SELECT CartId FROM Carts WHERE UserId=" + userid + " AND Ordered='true';";
        command.CommandText = query;
        await connection.OpenAsync();
        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            var cartid = (int)reader["CartId"];
            carts.Add(GetCart(cartid));
          }
        }
      }
      return carts;
    }

    /*
    Cart GetCart(int cartid)
    {
      var cart = new Cart();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        connection.Open();

        string query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            CartItem item = new CartItem()
            {
              Id = (int)reader["CartItemId"],
              Product = GetProduct((int)reader["ProductId"])
            };
            cart.CartItems.Add(item);
          }
        }

        query = "SELECT * FROM Carts WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            cart.Id = cartid;
            cart.User = GetUser((int)reader["UserId"]);
            cart.Ordered = bool.Parse((string)reader["Ordered"]);
            cart.OrderedOn = (string)reader["OrderedOn"];
          }
        }
      }
      return cart;
    }

    */

    public Cart GetCart(int cartid)
    {
      var cart = new Cart();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        connection.Open();

        string query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            CartItem item = new CartItem()
            {
              Id = (int)reader["CartItemId"],
              Product = GetProduct((int)reader["ProductId"])
            };
            cart.CartItems.Add(item);
          }
        }

        query = "SELECT * FROM Carts WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            cart.Id = cartid;
            cart.User = iUserDataAccess.GetUser((int)reader["UserId"]);
            cart.Ordered = bool.Parse((string)reader["Ordered"]);
            cart.OrderedOn = (string)reader["OrderedOn"];
          }
        }
      }
      return cart;
    }

    async Task<Cart> IDataAccess.GetActiveCartOfUser(int userid)
    {
      var cart = new Cart();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) From Carts WHERE UserId=" + userid + " AND Ordered='false';";
        command.CommandText = query;

        int count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
          return cart;
        }

        query = "SELECT CartId From Carts WHERE UserId=" + userid + " AND Ordered='false';";
        command.CommandText = query;

        int cartid = Convert.ToInt32(command.ExecuteScalar());

        query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
        command.CommandText = query;

        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            CartItem item = new CartItem()
            {
              Id = (int)reader["CartItemId"],
              Product = GetProduct((int)reader["ProductId"])
            };
            cart.CartItems.Add(item);
          }
        }

        cart.Id = cartid;
        cart.User = iUserDataAccess.GetUser(userid);
        cart.Ordered = false;
        cart.OrderedOn = "";
      }
      return cart;
    }

    async Task<List<Cart>> IDataAccess.GetAllPreviousCartsOfUser(int userid)
    {
      var carts = new List<Cart>();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        string query = "SELECT CartId FROM Carts WHERE UserId=" + userid + " AND Ordered='true';";
        command.CommandText = query;
        await connection.OpenAsync();
        using (MySqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            var cartid = (int)reader["CartId"];
            carts.Add(GetCart(cartid));
          }
        }
      }
      return carts;
    }

    async Task<List<PaymentMethod>> IDataAccess.GetPaymentMethods()
    {
      var result = new List<PaymentMethod>();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        string query = "SELECT * FROM PaymentMethods;";
        command.CommandText = query;

        await connection.OpenAsync();

        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          PaymentMethod paymentMethod = new PaymentMethod()
          {
            Id = (int)reader["PaymentMethodId"],
            Type = (string)reader["Type"],
            Provider = (string)reader["Provider"],
            Available = !Convert.IsDBNull(reader["Available"]) && ((string)reader["Available"]).Equals("Yes", StringComparison.OrdinalIgnoreCase),
            Reason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? null : (string)reader["Reason"]
          };
          result.Add(paymentMethod);
        }
      }
      return result;
    }


    public async Task<int> InsertPayment(Payment payment)
    {
      int value = 0;
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        string query = @"INSERT INTO Payments (PaymentMethodId, UserId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt) 
                        VALUES (@pmid, @uid, @ta, @sc, @ar, @ap, @cat);";

        command.CommandText = query;
        command.Parameters.Add("@pmid", MySqlDbType.Int32).Value = payment.PaymentMethod.Id;
        command.Parameters.Add("@uid", MySqlDbType.Int32).Value = payment.User.Id;
        command.Parameters.Add("@ta", MySqlDbType.VarChar).Value = payment.TotalAmount;
        command.Parameters.Add("@sc", MySqlDbType.VarChar).Value = payment.ShippingCharges;
        command.Parameters.Add("@ar", MySqlDbType.VarChar).Value = payment.AmountReduced;
        command.Parameters.Add("@ap", MySqlDbType.VarChar).Value = payment.AmountPaid;
        command.Parameters.Add("@cat", MySqlDbType.VarChar).Value = payment.CreatedAt;

        await connection.OpenAsync();
        value = command.ExecuteNonQuery();

        if (value > 0)
        {
          query = "SELECT Id FROM Payments ORDER BY Id DESC LIMIT 1;";
          command.CommandText = query;
          value = Convert.ToInt32(command.ExecuteScalar());
        }
        else
        {
          value = 0;
        }
      }
      return value;
    }


    public async Task<int> InsertOrder(Order order)
    {
      int value = 0;

      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };

        string query = "INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt) VALUES (@uid, @cid, @pid, @cat);";

        command.CommandText = query;
        command.Parameters.Add("@uid", MySqlDbType.Int32).Value = order.User.Id;
        command.Parameters.Add("@cid", MySqlDbType.Int32).Value = order.Cart.Id;
        command.Parameters.Add("@cat", MySqlDbType.VarChar).Value = order.CreatedAt;
        command.Parameters.Add("@pid", MySqlDbType.Int32).Value = order.Payment.Id;

        await connection.OpenAsync();
        value = command.ExecuteNonQuery();

        if (value > 0)
        {
          query = "UPDATE Carts SET Ordered='true', OrderedOn='" + DateTime.Now.ToString(dateformat) + "' WHERE CartId=" + order.Cart.Id + ";";
          command.CommandText = query;
          command.ExecuteNonQuery();

          query = "SELECT Id FROM Orders ORDER BY Id DESC LIMIT 1;";
          command.CommandText = query;
          value = Convert.ToInt32(command.ExecuteScalar());
        }
        else
        {
          value = 0;
        }
      }

      return value;
    }

    public async Task<int> InsertProduct(Product product)
    {
      var productCategory = new ProductCategory();
      productCategory = iCategoryDataAccess.GetProductCategory(product.ProductCategory.Id);

      if (productCategory == null)
      {
        return -1;
      }

      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        await connection.OpenAsync();

        string query = "INSERT INTO Products (Title, Description, CategoryId, OfferId, Price, Quantity, ImageName) " +
                       "VALUES (@title, @description, @categoryId, @offerId, @price, @quantity, @imageName);";

        command.CommandText = query;
        command.Parameters.AddWithValue("@title", product.Title);
        command.Parameters.AddWithValue("@description", product.Description);
        command.Parameters.AddWithValue("@categoryId", product.ProductCategory.Id);
        command.Parameters.AddWithValue("@offerId", product.Offer.Id);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@quantity", product.Quantity);
        command.Parameters.AddWithValue("@imageName", product.ImageName);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
          command.CommandText = "SELECT LAST_INSERT_ID();";
          int productId = Convert.ToInt32(command.ExecuteScalar());

          //upload image from imageFile
          if (!string.IsNullOrEmpty(product.ImageFile))
          {
            // Convert the Base64 string to a byte array
            byte[] fileBytes = Convert.FromBase64String(product.ImageFile);

            // Generate a unique filename using the current timestamp
            string timestamp = DateTime.Now.Ticks.ToString();
            string fileName = $"{productId}.jpg";

            // Define the base directory path and create it if it doesn't exist
            var baseDirectory = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "Resources", "Images");
            Directory.CreateDirectory(baseDirectory);

            // Get the category and subcategory folder paths
            var categoryFolder = Path.Combine(baseDirectory, productCategory.Category);
            var subcategoryFolder = Path.Combine(categoryFolder, productCategory.SubCategory);

            // Create category and subcategory folders if they don't exist
            Directory.CreateDirectory(categoryFolder);
            Directory.CreateDirectory(subcategoryFolder);

            // Define the folder path for the productId
            string productIdFolder = Path.Combine(subcategoryFolder, productId.ToString());
            Directory.CreateDirectory(productIdFolder);

            // Define the file path
            string filePath = Path.Combine(productIdFolder, fileName);

            // Save the byte array to a file
            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

            // Update the product's ImageName attribute with the file path
            product.ImageName = filePath;
          }

          // save the image path to imageName in mysql
          command.CommandText = "UPDATE Products SET ImageName = @imageName WHERE ProductId = @productId";
          command.Parameters.Clear();
          command.Parameters.AddWithValue("@imageName", product.ImageName);
          command.Parameters.AddWithValue("@productId", productId);
          command.ExecuteNonQuery();
          return productId;
        }
        else
        {
          return -1;
        }
      }
    }

    public async Task<bool> DeleteProduct(int id, string category, string subCategory)
    {
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection
        };
        await connection.OpenAsync();

        string query = "DELETE FROM Products WHERE ProductId = @id;";
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
          // Define the base directory path
          var baseDirectory = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "Resources", "Images");

          // Get the category and subcategory folder paths
          var categoryFolder = Path.Combine(baseDirectory, category);
          var subcategoryFolder = Path.Combine(categoryFolder, subCategory);

          // Define the folder path for the productId
          string productIdFolder = Path.Combine(subcategoryFolder, id.ToString());

          // Delete the folder if it exists
          if (Directory.Exists(productIdFolder))
          {
            Directory.Delete(productIdFolder, true);
          }

          return true;
        }
        return false;
      }
    }

    public async Task<List<Order>> GetAllOrders()
    {
      List<Order> orders = new List<Order>();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand
        {
          Connection = connection
        };

        string query = @"
    SELECT o.Id, o.CreatedAt,
        u.UserId AS UserId, u.FirstName, u.LastName, u.Email, u.Address, u.Mobile, u.Password, u.CreatedAt AS UserCreatedAt, u.ModifiedAt AS UserModifiedAt,
        c.CartId AS CartId, c.Ordered, c.OrderedOn,
        p.Id AS PaymentId, p.CreatedAt AS PaymentCreatedAt
    FROM Orders o
    JOIN Users u ON o.UserId = u.UserId
    JOIN Carts c ON o.CartId = c.CartId
    JOIN Payments p ON o.PaymentId = p.Id;";
        command.CommandText = query;

        await connection.OpenAsync();
        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          Order order = new Order
          {
            Id = (int)reader["Id"],
            CreatedAt = (string)reader["CreatedAt"],
            User = new User
            {
              Id = (int)reader["UserId"],
              FirstName = (string)reader["FirstName"],
              LastName = (string)reader["LastName"],
              Email = (string)reader["Email"],
              Address = (string)reader["Address"],
              Mobile = (string)reader["Mobile"],
              Password = (string)reader["Password"],
              CreatedAt = (string)reader["UserCreatedAt"],
              ModifiedAt = (string)reader["UserModifiedAt"]
            },
            Cart = new Cart
            {
              Id = (int)reader["CartId"],
              Ordered = Convert.ToBoolean(reader["Ordered"]),
              OrderedOn = (string)reader["OrderedOn"]
            },
            Payment = new Payment
            {
              Id = (int)reader["PaymentId"],
              CreatedAt = (string)reader["PaymentCreatedAt"]
            }
          };

          orders.Add(order);
        }
      }
      return orders;
    }

  }
}
