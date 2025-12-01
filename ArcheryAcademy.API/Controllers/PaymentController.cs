using ArcheryAcademy.Application.Dtos.PaymentDto;
using ArcheryAcademy.Application.UseCases.PaymentUseCase.Queries;
using ArcheryAcademy.Application.UseCases.PaymentUseCases.Command;
using ArcheryAcademy.Application.UseCases.PaymentUseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IMediator mediator, IMapper mapper) : ControllerBase 
{
    // GET: api/payment
    [HttpGet]
    public async Task<ActionResult<List<PaymentReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllPaymentsQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetPaymentByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = $"Payment with ID {id} not found." });

        return Ok(result);
    }

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto dto)
    {
        var command = new CreatePaymentCommand(dto);
        
        var createdEntity = await mediator.Send(command); 
        
        var resultDto = mapper.Map<PaymentReadDto>(createdEntity);
        
        // 201 CreatedAtAction con la URI y el DTO
        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    // PUT (Actualizar)
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] PaymentUpdateDto dto)
    {
        // Nota: Si el DTO de Update no tiene ID, se omite esta validación.
        // Si lo tuviera, sería: if (id != dto.Id) return BadRequest(...);

        var command = new UpdatePaymentCommand(id, dto);
        var updatedEntity = await mediator.Send(command); 

        if (updatedEntity is null)
            return NotFound($"Payment with ID {id} not found.");
        
        var resultDto = mapper.Map<PaymentReadDto>(updatedEntity);

        return Ok(resultDto);
    }
    
    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeletePaymentCommand(id));

        // El Command devuelve 'true' si se eliminó, 'false' si no se encontró
        if (!result)
            return NotFound($"Payment with ID {id} was not found.");

        return NoContent(); // 204 No Content para eliminaciones exitosas
    }
    
    // GET: api/payment/report
    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var query = new GetPaymentReportQuery(startDate, endDate);

        var fileResult = await mediator.Send(query);

        return File(fileResult.FileContent, fileResult.ContentType, fileResult.FileName);
    }
}