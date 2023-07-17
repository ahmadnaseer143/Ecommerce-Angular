using ecommerce.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OffersController : ControllerBase
  {
    readonly IOfferDataAccess dataAccess;
    private readonly string DateFormat;

    public OffersController(IOfferDataAccess dataAccess, IConfiguration configuration)
    {
      this.dataAccess = dataAccess;
      DateFormat = configuration["Constants:DateFormat"];

    }

    [HttpGet("GetOffer/{id}")]
    public IActionResult GetOffer(int id)
    {
      var result = dataAccess.GetOffer(id);
      return Ok(result);
    }

    [HttpGet("GetAllOffers")]

    public async Task<IActionResult> GetAllOffers()
    {
      var result = await dataAccess.GetAllOffers();
      return Ok(result);
    }
  }
}
