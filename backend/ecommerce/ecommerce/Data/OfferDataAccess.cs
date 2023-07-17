using ecommerce.Data.Interfaces;
using ecommerce.Models;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace ecommerce.Data
{
  public class OfferDataAccess : IOfferDataAccess
  {
    private readonly IConfiguration configuration;
    private readonly string dbConnection;
    public OfferDataAccess(IConfiguration configuration)
    {
      this.configuration = configuration;
      dbConnection = this.configuration.GetConnectionString("DB");
    }

    public Offer GetOffer(int id)
    {
      var offer = new Offer();
      using (MySqlConnection connection = new(dbConnection))
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

    public async Task<List<Offer>> GetAllOffers()
    {
      List<Offer> offers = new List<Offer>();
      using (MySqlConnection connection = new MySqlConnection(dbConnection))
      {
        MySqlCommand command = new MySqlCommand()
        {
          Connection = connection,
        };

        string query = "SELECT * FROM Offers;";
        command.CommandText = query;

        await connection.OpenAsync();

        DbDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          Offer offer = new Offer()
          {
            Id = (int)reader["OfferId"],
            Title = (string)reader["Title"],
            Discount = (int)reader["Discount"]
          };

          offers.Add(offer);
        }
      }
      return offers;
    }
  }
}
