using ecommerce.Models;

namespace ecommerce.Data
{
  public interface IDataAccess
  {
    List<ProductCategory> GetProductCategories();
  }
}
