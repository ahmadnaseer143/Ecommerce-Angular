using ecommerce.Data;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ShoppingController : ControllerBase
  {
    readonly IDataAccess dataAccess;
    private readonly string DateFormat;

    public ShoppingController(IDataAccess dataAccess, IConfiguration configuration)
    {
      this.dataAccess = dataAccess;
      DateFormat = configuration["Constants:DateFormat"];

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


    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser([FromBody] User user)
    {
      user.CreatedAt = DateTime.Now.ToString(DateFormat);
      user.ModifiedAt = DateTime.Now.ToString(DateFormat);

      var result = dataAccess.InsertUser(user);

      string? message;
      if (result) message = "Account Created";
      else message = "Email already taken";
      return Ok(message);
    }

    [HttpPost("LoginUser")]
    public IActionResult LoginUser([FromBody] User user)
    {
      var token = dataAccess.IsUserPresent(user.Email, user.Password);
      if (token == "") token = "invalid";
      return Ok(token);
    }
  }
}
