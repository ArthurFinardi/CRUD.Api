using Microsoft.AspNetCore.Mvc;
using CRUD.Api.Models;

namespace CRUD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private static List<Customer> _clientes = new List<CustomerDto>();
        private static int _idCounter = 1;

        /// <summary>
        /// Retorna todos os clientes cadastrados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CustomerDto>> Get()
        {
            return Ok(_clientes);
        }

        /// <summary>
        /// Retorna um cliente específico pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDto> Get(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado" });

            return Ok(cliente);
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CustomerDto> Post(CustomerDto cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_clientes.Any(c => c.CPF == cliente.Document))
                return BadRequest(new { message = "Já existe um cliente cadastrado com este CPF" });

            cliente.Id = _idCounter++;
            cliente.Date = DateTime.Now;
            _clientes.Add(cliente);

            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, CustomerDto cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteExistente = _clientes.FirstOrDefault(c => c.Id == id);
            if (clienteExistente == null)
                return NotFound(new { message = "Cliente não encontrado" });

            if (_clientes.Any(c => c.CPF == cliente.Document && c.Id != id))
                return BadRequest(new { message = "Já existe outro cliente cadastrado com este CPF" });

            clienteExistente.Nome = cliente.Name;
            clienteExistente.CPF = cliente.Document;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Phone;

            return NoContent();
        }

        /// <summary>
        /// Remove um cliente
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado" });

            _clientes.Remove(cliente);
            return NoContent();
        }
    }
} 