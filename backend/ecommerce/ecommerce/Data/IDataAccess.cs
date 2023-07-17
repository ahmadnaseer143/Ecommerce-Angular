using ecommerce.Models;

namespace ecommerce.Data
{
  public interface IDataAccess
  {
    Task<List<PaymentMethod>> GetPaymentMethods();
    Task<int> InsertPayment(Payment payment);
    Task<int> InsertOrder(Order order);

    Task<List<Order>> GetAllOrders();
  }
}
