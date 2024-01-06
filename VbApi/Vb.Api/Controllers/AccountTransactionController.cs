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
        public async Task<ActionResult<ApiResponse<List<AccountTransactionResponse>>>> GetAllAddresses()
        {
            var query = new GetAllAccountTransactionQuery();
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AddressResponse>> direkt olarak döndürülüyor.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AccountTransactionResponse>>> GetAccountTransactionById(int id)
        {
            var query = new GetAccountTransactionByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<AccountTransactionResponse>>>> GetAccountTransactionByParameter([FromQuery] string customerName, [FromQuery] string address1, [FromQuery] string address2)
        {
            var query = new GetAccountTransactionByParameterQuery(customerName, address1, address2);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AddressResponse>> direkt olarak döndürülüyor.
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AccountTransactionResponse>>> CreateAccountTransaction([FromBody] AccountTransactionRequest address)
        {
            var command = new CreateAccountTransactionCommand(address);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponse>>> UpdateAccountTransaction(int id, [FromBody] AccountTransactionRequest address)
        {
            var command = new UpdateAccountTransactionCommand(id, address);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AddressResponse> direkt olarak döndürülüyor.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAccountTransaction(int id)
        {
            var command = new DeleteAccountTransactionCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse direkt olarak döndürülüyor.
        }
    }
}
