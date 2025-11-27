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

    // PUT (Actualizar)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] PaymentMethodUpdateDto dto)
    {
        var command = new UpdatePaymentMethodCommand(id, dto);
        var updatedEntity = await mediator.Send(command);

        if (updatedEntity is null)
            return NotFound($"PaymentMethod with ID {id} not found.");

        var resultDto = mapper.Map<PaymentMethodReadDto>(updatedEntity);
        return Ok(resultDto);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeletePaymentMethodCommand(id));

        if (!result)
            return NotFound($"PaymentMethod with ID {id} was not found.");

        return NoContent();
    }
}