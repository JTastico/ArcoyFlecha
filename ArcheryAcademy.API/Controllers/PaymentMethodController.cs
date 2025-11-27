using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController(IMediator mediator) : ControllerBase
{
    // GET: api/paymentmethod
    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllPaymentMethodsQuery());
        return Ok(result);
    }
}