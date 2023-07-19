using ecommerce.Data.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
      try
      {
        var result = await dataAccess.GetBannerImage(name);
        return Ok(result);
      }
      catch (Exception ex)
      {
        // Log the exception or handle it appropriately
        Console.WriteLine($"Error while retrieving the image: {ex.Message}");
        return StatusCode(500, "Internal Server Error");
      }
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
  }
}
