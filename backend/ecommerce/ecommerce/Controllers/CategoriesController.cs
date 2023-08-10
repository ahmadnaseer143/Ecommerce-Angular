using ecommerce.Data.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {

    readonly ICategoryDataAccess dataAccess;

    public CategoriesController(ICategoryDataAccess dataAccess)
    {
      this.dataAccess = dataAccess;
    }

    [HttpGet("GetCategoryList")]

    public async Task<IActionResult> GetCategoryList()
    {
      var result = await dataAccess.GetProductCategories();
      return Ok(result);
    }

    [HttpGet("GetBannerImage")]
    public async Task<IActionResult> GetBannerImage(string name)
    {
      // Retrieve the image file path based on the productId
      byte[] imageBytes = await dataAccess.GetBannerImage(name);

      if (imageBytes == null || imageBytes.Length == 0)
      {
        return NotFound();
      }

      // Return the image file as the response with the appropriate content type
      return File(imageBytes, "image/png");
    }

    [HttpPost("InsertCategory")]
    public async Task<IActionResult> InsertCategory(ProductCategory productCategory)
    {
      var result = await dataAccess.InsertProductCategory(productCategory);

      if (result == "ok")
      {
        return Ok(result);
      }
      else
      {
        return BadRequest(new { error = "Failed to insert product category.", msg = result });
      }
    }


    //[HttpPost("InsertCategory")]
    //public async Task<IActionResult> InsertCategory([FromForm] ProductCategory productCategory, IFormFile photoFile)
    //{
    //  if (photoFile == null || photoFile.Length == 0)
    //  {
    //    return BadRequest("No photo file found.");
    //  }

    //  var result = await dataAccess.InsertProductCategory(productCategory, photoFile);
    //  if (result)
    //  {
    //    return Ok(result);
    //  }
    //  else
    //  {
    //    return BadRequest("Failed to insert product category.");
    //  }
    //}

    [HttpPut("EditCategory")]
    public async Task<IActionResult> EditCategory(ProductCategory category)
    {

        string result = await dataAccess.UpdateCategory(category);
        if (result=="ok")
        {
            return Ok(result);
        }
        else
        {
        return BadRequest(new { error = "Failed to update category.", msg = result });
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
  }
}
