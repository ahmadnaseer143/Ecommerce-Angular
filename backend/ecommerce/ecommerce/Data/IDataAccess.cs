using ecommerce.Models;
using ECommerce.API.Models;

namespace ecommerce.Data
{
  public interface IDataAccess
  {
    List<ProductCategory> GetProductCategories();
    ProductCategory GetProductCategory(int id);

    Offer GetOffer(int id);

    List<Product> GetProducts(string category, string subCategory, int count);

    Product GetProduct(int id);

    bool InsertUser(User user);

    string IsUserPresent(string email, string password);

    void InsertReview(Review review);

    List<Review> GetProductReviews(int productId);

    User GetUser(int id);
  }
}
