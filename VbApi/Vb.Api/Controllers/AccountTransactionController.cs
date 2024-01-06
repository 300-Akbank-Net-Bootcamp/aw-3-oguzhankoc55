using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<AddressResponse>>>> GetAllAddresses()
        {
            var query = new GetAllAddressQuery();
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AddressResponse>> direkt olarak döndürülüyor.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponse>>> GetAddressById(int id)
        {
            var query = new GetAddressByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<AddressResponse>>>> GetAddressesByParameter([FromQuery] string customerName, [FromQuery] string address1, [FromQuery] string address2)
        {
            var query = new GetAddressByParameterQuery(customerName, address1, address2);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AddressResponse>> direkt olarak döndürülüyor.
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddressResponse>>> CreateAddress([FromBody] AddressRequest address)
        {
            var command = new CreateAddressCommand(address);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponse>>> UpdateAddress(int id, [FromBody] AddressRequest address)
        {
            var command = new UpdateAdressCommand(id, address);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAddress(int id)
        {
            var command = new DeleteAddressCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse direkt olarak döndürülüyor.
        }
    }
}
