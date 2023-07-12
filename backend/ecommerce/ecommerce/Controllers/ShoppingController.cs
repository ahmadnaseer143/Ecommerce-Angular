using ecommerce.Data;
using ecommerce.Models;
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

    public async Task<IActionResult> GetCategoryList()
    {
      var result = await dataAccess.GetProductCategories();
      return Ok(result);
    }

    [HttpPost("InsertCategory")]
    public async Task<IActionResult> InsertCategory(ProductCategory productCategory)
    {
      var result = await dataAccess.InsertProductCategory(productCategory);
      if (result)
      {
        return Ok(result);
      }
      else
      {
        return BadRequest("Failed to insert product category.");
      }
    }

    [HttpPut("EditCategory")]
    public async Task<IActionResult> EditCategory(ProductCategory category)
    {
      bool isUpdated = await dataAccess.UpdateCategory(category);
      if (isUpdated)
      {
        return Ok(isUpdated);
      }
      else
      {
        return BadRequest("Failed to update category.");
      }
    }


    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      var boolValue = await dataAccess.DeleteProductCategory(id);
      if (boolValue)
      {
        return Ok(true);
      }
      else
      {
        return NotFound();
      }
    }



    [HttpGet("GetProducts")]

    public IActionResult GetProducts(string category, string subCategory, int count)
    {
      var result = dataAccess.GetProducts(category, subCategory, count);
      return Ok(result);
    }

    [HttpGet("GetAllProducts")]

    public IActionResult GetAllProducts()
    {
      var result = dataAccess.GetAllProducts();
      return Ok(result);
    }

    [HttpGet("GetProduct/{id}")]

    public IActionResult GetProduct(int id)
    {
      var result = dataAccess.GetProduct(id);
      return Ok(result);
    }

    [HttpPut("UpdateProduct")]

    public IActionResult UpdateProduct(Product product)
    {
      var result = dataAccess.UpdateProduct(product);
      if(result != null)
      {
      return Ok(result);
      }
      return BadRequest();
    }

    [HttpGet("GetAllOffers")]

    public IActionResult GetAllOffers()
    {
      var result = dataAccess.GetAllOffers();
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

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await dataAccess.GetAllUsers();

      return Ok(users);
    }

    [HttpPost("InsertReview")]
    public IActionResult InsertReview([FromBody] Review review)
    {
      review.CreatedAt = DateTime.Now.ToString(DateFormat);
      dataAccess.InsertReview(review);
      return Ok("Review inserted");
    }

    [HttpGet("GetProductReviews/{productId}")]
    public IActionResult GetProductReviews(int productId)
    {
      var result = dataAccess.GetProductReviews(productId);
      return Ok(result);
    }

    [HttpPost("InsertCartItem/{userid}/{productid}")]
    public IActionResult InsertCartItem(int userid, int productid)
    {
      var result = dataAccess.InsertCartItem(userid, productid);
      return Ok(result ? "inserted" : "not inserted");
    }

    [HttpPost("RemoveCartItem/{userid}/{productid}")]
    public IActionResult RemoveCartItem(int userid, int productid)
    {
      var result = dataAccess.RemoveCartItem(userid, productid);
      return Ok(result ? "removed" : "not removed");
    }




    [HttpGet("GetActiveCartOfUser/{id}")]
    public IActionResult GetActiveCartOfUser(int id)
    {
      var result = dataAccess.GetActiveCartOfUser(id);
      return Ok(result);
    }

    [HttpGet("GetAllPreviousCartsOfUser/{id}")]
    public IActionResult GetAllPreviousCartsOfUser(int id)
    {
      var result = dataAccess.GetAllPreviousCartsOfUser(id);
      return Ok(result);
    }

    [HttpGet("GetPaymentMethods")]
    public IActionResult GetPaymentMethods()
    {
      var result = dataAccess.GetPaymentMethods();
      return Ok(result);
    }

    [HttpPost("InsertPayment")]
    public IActionResult InsertPayment(Payment payment)
    {
      payment.CreatedAt = DateTime.Now.ToString();
      var id = dataAccess.InsertPayment(payment);
      return Ok(id.ToString());
    }

    [HttpPost("InsertOrder")]
    public IActionResult InsertOrder(Order order)
    {
      order.CreatedAt = DateTime.Now.ToString();
      var id = dataAccess.InsertOrder(order);
      return Ok(id.ToString());
    }

    [HttpPost("InsertProduct")]
    public IActionResult InsertProduct(Product product)
    {
      var boolValue = dataAccess.InsertProduct(product);
      if (boolValue > 0) {
      return Ok(boolValue);
      }
       return BadRequest();
    }

    [HttpDelete("DeleteProduct/{id}")]

    public IActionResult DeleteProduct(int id)
    {
      var boolValue = dataAccess.DeleteProduct(id);
      if (boolValue == false)
      {
        return NotFound();
      }

      return Ok(true);
    }

    [HttpGet("GetAllOrders")]
    public IActionResult GetAllOrders()
    {
      var orders = dataAccess.GetAllOrders();
      if (orders == null || orders.Count == 0)
      {
        return NotFound();
      }

      return Ok(orders);
    }

  }
}
