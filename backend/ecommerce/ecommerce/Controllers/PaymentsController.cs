using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentsController : ControllerBase
  {
    readonly IDataAccess dataAccess;
    private readonly string DateFormat;

    public PaymentsController(IDataAccess dataAccess, IConfiguration configuration)
    {
      this.dataAccess = dataAccess;
      DateFormat = configuration["Constants:DateFormat"];

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
  }
}
