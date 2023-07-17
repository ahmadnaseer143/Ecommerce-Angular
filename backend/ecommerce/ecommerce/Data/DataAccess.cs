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
    public DataAccess(IConfiguration configuration, IWebHostEnvironment hostEnvironment, ICategoryDataAccess iCategoryDataAccess, IOfferDataAccess iOfferDataAccess, IUserDataAccess iUserDataAccess)
    {
      this.configuration = configuration;
      dbConnection = this.configuration.GetConnectionString("DB");
      dateformat = this.configuration["Constants:DateFormat"];
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
