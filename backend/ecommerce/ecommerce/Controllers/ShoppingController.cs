using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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

    public async Task<IActionResult> GetProducts(string category, string subCategory, int count)
    {
      var result = await dataAccess.GetProducts(category, subCategory, count);
      return Ok(result);
    }

    [HttpGet("GetImage/{productId}")]
    public async Task<IActionResult> GetImage(int productId)
    {
      // Retrieve the image file path based on the productId
      byte[] imageBytes = await dataAccess.GetProductImage(productId);

      if (imageBytes == null || imageBytes.Length == 0)
      {
        return NotFound();
      }

      // Return the image file as the response with the appropriate content type
      return File(imageBytes, "image/jpg");
    }


    [HttpGet("GetAllProducts")]

    public async Task<IActionResult> GetAllProducts()
    {
      var result = await dataAccess.GetAllProducts();
      return Ok(result);
    }

    [HttpGet("GetProduct/{id}")]

    public IActionResult GetProduct(int id)
    {
      var result = dataAccess.GetProduct(id);
      return Ok(result);
    }

    [HttpPut("UpdateProduct")]

    public async Task<IActionResult> UpdateProduct(Product product)
    {
      var result = await dataAccess.UpdateProduct(product);
      if(result != null)
      {
      return Ok(result);
      }
      return BadRequest();
    }

    [HttpGet("GetAllOffers")]

    public async Task<IActionResult> GetAllOffers()
    {
      var result = await dataAccess.GetAllOffers();
      return Ok(result);
    }


    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
      user.CreatedAt = DateTime.Now.ToString(DateFormat);
      user.ModifiedAt = DateTime.Now.ToString(DateFormat);

      var result = await dataAccess.InsertUser(user);

      string? message;
      if (result) message = "Account Created";
      else message = "Email already taken";
      return Ok(message);
    }

    [HttpPost("LoginUser")]
    public async Task<IActionResult> LoginUser([FromBody] User user)
    {
      var token = await dataAccess.IsUserPresent(user.Email, user.Password);
      if (token == "") token = "invalid";
      return Ok(token);
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await dataAccess.GetAllUsers();

      return Ok(users);
    }

    [HttpPost("InsertReview")]
    public async Task<IActionResult> InsertReview([FromBody] Review review)
    {
      review.CreatedAt = DateTime.Now.ToString(DateFormat);
      dataAccess.InsertReview(review);
      return Ok("Review inserted");
    }

    [HttpGet("GetProductReviews/{productId}")]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
      var result = await dataAccess.GetProductReviews(productId);
      return Ok(result);
    }

    [HttpPost("InsertCartItem/{userid}/{productid}")]
    public async Task<IActionResult> InsertCartItem(int userid, int productid)
    {
      var result = await dataAccess.InsertCartItem(userid, productid);
      return Ok(result ? "inserted" : "not inserted");
    }

    [HttpPost("RemoveCartItem/{userid}/{productid}")]
    public async Task<IActionResult> RemoveCartItem(int userid, int productid)
    {
      var result = await dataAccess.RemoveCartItem(userid, productid);
      return Ok(result ? "removed" : "not removed");
    }




    [HttpGet("GetActiveCartOfUser/{id}")]
    public async Task<IActionResult> GetActiveCartOfUser(int id)
    {
      var result = await dataAccess.GetActiveCartOfUser(id);
      return Ok(result);
    }

    [HttpGet("GetAllPreviousCartsOfUser/{id}")]
    public async Task<IActionResult> GetAllPreviousCartsOfUser(int id)
    {
      var result = await dataAccess.GetAllPreviousCartsOfUser(id);
      return Ok(result);
    }

    [HttpGet("GetPaymentMethods")]
    public async Task<IActionResult> GetPaymentMethods()
    {
      var result = await dataAccess.GetPaymentMethods();
      return Ok(result);
    }

    [HttpPost("InsertPayment")]
    public async Task<IActionResult> InsertPayment(Payment payment)
    {
      payment.CreatedAt = DateTime.Now.ToString();
      var id = await dataAccess.InsertPayment(payment);
      return Ok(id.ToString());
    }

    [HttpPost("InsertOrder")]
    public async Task<IActionResult> InsertOrder(Order order)
    {
      order.CreatedAt = DateTime.Now.ToString();
      var id = await dataAccess.InsertOrder(order);
      return Ok(id.ToString());
    }

    [HttpPost("InsertProduct")]
    public async Task<IActionResult> InsertProduct(Product product)
    {
      var boolValue = await dataAccess.InsertProduct(product);

      if (boolValue > 0)
      {
        return Ok(boolValue);
      }

      return BadRequest();
    }


    [HttpDelete("DeleteProduct/{id}")]

    public async Task<IActionResult> DeleteProduct(int id, string category, string subCategory)
    {
      var boolValue = await dataAccess.DeleteProduct(id, category, subCategory);
      if (boolValue == false)
      {
        return NotFound();
      }

      return Ok(true);
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
      var orders = await dataAccess.GetAllOrders();
      if (orders == null || orders.Count == 0)
      {
        return NotFound();
      }

      return Ok(orders);
    }

  }
}
