using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Queries;
using ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Command;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController(IMediator mediator, IMapper mapper) : ControllerBase
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

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodCreateDto dto)
    {
        var command = new CreatePaymentMethodCommand(dto);
        var createdEntity = await mediator.Send(command);

        var resultDto = mapper.Map<PaymentMethodReadDto>(createdEntity);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }
}