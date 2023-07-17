using ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OffersController : ControllerBase
  {
    readonly IDataAccess dataAccess;
    private readonly string DateFormat;

    public OffersController(IDataAccess dataAccess, IConfiguration configuration)
    {
      this.dataAccess = dataAccess;
      DateFormat = configuration["Constants:DateFormat"];

    }

    [HttpGet("GetAllOffers")]

    public async Task<IActionResult> GetAllOffers()
    {
      var result = await dataAccess.GetAllOffers();
      return Ok(result);
    }
  }
}
