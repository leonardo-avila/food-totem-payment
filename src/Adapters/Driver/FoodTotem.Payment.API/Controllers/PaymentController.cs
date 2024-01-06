using FoodTotem.Domain.Core;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentUseCases _paymentUseCases;

    public PaymentController(IPaymentUseCases orderUseCases)
    {
        _paymentUseCases = orderUseCases;
    }

    /// <summary>
    /// Get payment by order reference
    /// </summary>
    /// <param name="orderReference"></param>
    /// <returns>Returns the payment with the specified id</returns>
    /// <response code="204">No payment with the specified id was found.</response>
    [HttpGet("{orderReference}", Name = "Get payment by order reference")]
    public async Task<ActionResult<PaymentViewModel>> GetByOrderReference(string orderReference)
    {
        try
        {
            return Ok(await _paymentUseCases.GetPaymentByOrderReference(orderReference));
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving payment.");
        }
    }


    /// <summary>
    /// Get payments
    /// </summary>
    /// <returns>Returns the payment with the specified id</returns>
    /// <response code="204">No payment with the specified id was found.</response>
    [HttpGet(Name = "Get payments")]
    public async Task<ActionResult<IEnumerable<PaymentViewModel>>> Get()
    {
        try
        {
            return Ok(await _paymentUseCases.GetPayments());
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving payment.");
        }
    }

    /// <summary>
    /// Create payment
    /// </summary>
    /// <param name="orderViewModel"></param>
    /// <returns>Returns the payment created for the order</returns>
    /// <response code="500">Something wrong happened when trying to create payment.</response>
    [HttpPost(Name = "Create payment")]
    public async Task<ActionResult<PaymentViewModel>> CreatePayment(OrderViewModel orderViewModel)
    {
        try
        {
            return Ok(await _paymentUseCases.CreatePayment(orderViewModel));
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating payment.");
        }
    }
}
