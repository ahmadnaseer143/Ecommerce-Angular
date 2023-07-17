using ecommerce.Models;

namespace ecommerce.Data
{
  public interface IDataAccess
  {
    Offer GetOffer(int id);

    Task<List<Offer>> GetAllOffers();

    Task<List<Product>> GetProducts(string category, string subCategory, int count);

    Task<byte[]> GetProductImage(int productId);

    Task<List<Product>> GetAllProducts();

    Product GetProduct(int id);

    Task<Product> UpdateProduct(Product product);

    Task<bool> InsertUser(User user);

    Task<string> IsUserPresent(string email, string password);

    Task InsertReview(Review review);

    Task<List<Review>> GetProductReviews(int productId);

    User GetUser(int id);

    Task<List<User>> GetAllUsers();

    Task<bool> InsertCartItem(int userId, int productId);

    Task<bool> RemoveCartItem(int userId, int productId);
    Task<Cart> GetActiveCartOfUser(int userid);
    Cart GetCart(int cartid);
    Task<List<Cart>> GetAllPreviousCartsOfUser(int userid);

    Task<List<PaymentMethod>> GetPaymentMethods();
    Task<int> InsertPayment(Payment payment);
    Task<int> InsertOrder(Order order);

    Task<int> InsertProduct(Product product);

    Task<bool> DeleteProduct(int id, string category, string subCategory);

    Task<List<Order>> GetAllOrders();
  }
}
