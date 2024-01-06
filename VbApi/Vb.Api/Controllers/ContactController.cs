using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ContactResponse>>>> GetAllContacts()
        {
            var query = new GetAllContactQuery();
            var result = await _mediator.Send(query);
            return result;
        }
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<ContactResponse>>>> GetContactsByParameter([FromQuery] int CustomerId, [FromQuery] string ContactType)
        {
            var query = new GetAddressByParameterQuery(CustomerId, ContactType);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AddressResponse>> direkt olarak döndürülüyor.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ContactResponse>>> GetContactById(int id)
        {
            var query = new GetContactByIdQuery(id);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ContactResponse>>> CreateContact([FromBody] ContactRequest Contact)
        {
            var command = new CreateContactCommand(Contact);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateContact(int id, [FromBody] ContactRequest Contact)
        {
            var command = new UpdateContactCommand(id, Contact);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteContact(int id)
        {
            var command = new DeleteContactCommand(id);
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
