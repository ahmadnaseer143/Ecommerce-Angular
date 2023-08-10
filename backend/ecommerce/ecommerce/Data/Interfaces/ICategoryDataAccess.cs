using ecommerce.Models;

namespace ecommerce.Data.Interfaces
{
  public interface ICategoryDataAccess
  {
    Task<List<ProductCategory>> GetProductCategories();

    Task<string> InsertProductCategory(ProductCategory productCategory);

    Task<bool> UpdateCategory(ProductCategory category, IFormFile photoFile);
    Task<bool> DeleteProductCategory(int id);
    ProductCategory GetProductCategory(int id);

    Task<byte[]> GetBannerImage(string name);
  }
}
