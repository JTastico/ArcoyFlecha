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

    // GET by id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetPaymentMethodByIdQuery(id));

        if (result == null)
            return NotFound(new { message = $"PaymentMethod with ID {id} not found." });

        return Ok(result);
    }
}