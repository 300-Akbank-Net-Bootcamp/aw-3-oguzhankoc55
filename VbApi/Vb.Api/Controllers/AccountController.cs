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
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<AccountResponse>>>> GetAllAccount()
        {
            var query = new GetAllAccountQuery();
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AccountResponse>> direkt olarak döndürülüyor.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> GetAccountById(int id)
        {
            var query = new GetAccountByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<AccountResponse> direkt olarak döndürülüyor.
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<AccountResponse>>>> GetAccountByParameter([FromQuery] int AccountNumber, [FromQuery] string IBAN, [FromQuery] decimal Balance, [FromQuery] string CurrencyType, [FromQuery] string Name, [FromQuery] DateTime OpenDate)
        {
            var query = new GetAccountByParameterQuery(AccountNumber, IBAN, Balance, CurrencyType, Name, OpenDate);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<AccountResponse>> direkt olarak döndürülüyor.
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> CreateAccount([FromBody] AccountRequest Account)
        {
            var command = new CreateAccountCommand(Account);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AccountResponse> direkt olarak döndürülüyor.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> UpdateAccount(int id, [FromBody] AccountRequest Account)
        {
            var command = new UpdateAdressCommand(id, Account);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<AccountResponse> direkt olarak döndürülüyor.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAccount(int id)
        {
            var command = new DeleteAccountCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse direkt olarak döndürülüyor.
        }
    }
}
