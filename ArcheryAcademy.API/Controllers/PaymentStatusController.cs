using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Command;
using ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentStatusController(IMediator mediator, IMapper mapper) : ControllerBase
{
    // GET: api/paymentstatus
    [HttpGet]
    public async Task<ActionResult<List<PaymentStatusReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllPaymentStatusesQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetPaymentStatusByIdQuery(id));

        if (result == null)
            return NotFound(new { message = $"PaymentStatus with ID {id} not found." });

        return Ok(result);
    }

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreatePaymentStatus([FromBody] PaymentStatusCreateDto dto)
    {
        var command = new CreatePaymentStatusCommand(dto);
        var createdEntity = await mediator.Send(command);

        var resultDto = mapper.Map<PaymentStatusReadDto>(createdEntity);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    // PUT (Actualizar)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatusUpdateDto dto)
    {
        var command = new UpdatePaymentStatusCommand(id, dto);
        var updatedEntity = await mediator.Send(command);

        if (updatedEntity is null)
            return NotFound($"PaymentStatus with ID {id} not found.");

        var resultDto = mapper.Map<PaymentStatusReadDto>(updatedEntity);
        return Ok(resultDto);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeletePaymentStatusCommand(id));

        if (!result)
            return NotFound($"PaymentStatus with ID {id} was not found.");

        return NoContent();
    }
}