using ecommerce.Models;

namespace ecommerce.Data
{
  public interface IDataAccess
  {
    Task<List<ProductCategory>> GetProductCategories();

    Task<bool> InsertProductCategory(ProductCategory productCategory);

    Task<bool> UpdateCategory(ProductCategory category);
    Task<bool> DeleteProductCategory(int id);
    ProductCategory GetProductCategory(int id);

    Offer GetOffer(int id);

    List<Offer> GetAllOffers();

    List<Product> GetProducts(string category, string subCategory, int count);

    List<Product> GetAllProducts();

    Product GetProduct(int id);

    Product UpdateProduct(Product product);

    bool InsertUser(User user);

    string IsUserPresent(string email, string password);

    void InsertReview(Review review);

    List<Review> GetProductReviews(int productId);

    User GetUser(int id);

    bool InsertCartItem(int userId, int productId);

    bool RemoveCartItem(int userId, int productId);
    Cart GetActiveCartOfUser(int userid);
    Cart GetCart(int cartid);
    List<Cart> GetAllPreviousCartsOfUser(int userid);

    List<PaymentMethod> GetPaymentMethods();
    int InsertPayment(Payment payment);
    int InsertOrder(Order order);

    bool InsertProduct(Product product);

    bool DeleteProduct(int id);

    List<Order> GetAllOrders();
  }
}
