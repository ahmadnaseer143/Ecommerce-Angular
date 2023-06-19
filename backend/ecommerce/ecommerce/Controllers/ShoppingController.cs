using ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ShoppingController : ControllerBase
  {
    readonly IDataAccess dataAccess;

    public ShoppingController(IDataAccess dataAccess, IConfiguration configuration)
    {
      this.dataAccess = dataAccess;

    }

    [HttpGet("GetCategoryList")]

    public IActionResult GetCategoryList()
    {
      var result = dataAccess.GetProductCategories();
      return Ok(result);
    }

    [HttpGet("GetProducts")]

    public IActionResult GetProducts(string category, string subCategory, int count)
    {
      Console.WriteLine(category);
      var result = dataAccess.GetProducts(category, subCategory, count);
      return Ok(result);
    }

    [HttpGet("GetProduct/{id}")]

    public IActionResult GetProduct(int id)
    {
      var result = dataAccess.GetProduct(id);
      return Ok(result);
    }
  }
}
