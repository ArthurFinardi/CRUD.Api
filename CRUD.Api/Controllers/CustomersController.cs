using System;
using System.Threading.Tasks;
using CRUD.Application.Customers.Commands.CreateCustomer;
using CRUD.Application.Customers.Commands.DeleteCustomer;
using CRUD.Application.Customers.Commands.UpdateCustomer;
using CRUD.Application.Customers.Queries.GetCustomer;
using CRUD.Application.Customers.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os clientes cadastrados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetCustomersViewModel), 200)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCustomersQuery());
            return Ok(result);
        }

        /// <summary>
        /// Retorna um cliente específico pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetCustomerViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerQuery { Id = id });
            if (result == null)
                return NotFound(new { message = "Cliente não encontrado" });

            return Ok(result);
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateCustomerCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(Get), new { id = result }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            try
            {
                command.Id = id;
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Cliente não encontrado.")
                    return NotFound(new { message = ex.Message });
                
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove um cliente
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteCustomerCommand { Id = id });
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
} 