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
    public class EftTransactionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EftTransactionesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EftTransactionResponse>>>> GetAllEftTransactiones()
        {
            var query = new GetAllEftTransactionQuery();
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<EftTransactionResponse>> direkt olarak döndürülüyor.
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EftTransactionResponse>>> GetEftTransactionById(int id)
        {
            var query = new GetEftTransactionByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<EftTransactionResponse> direkt olarak döndürülüyor.
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<EftTransactionResponse>>>> GetEftTransactionesByParameter([FromQuery] string ReferenceNumber, [FromQuery] DateTime TransactionDate, [FromQuery] decimal Amount, [FromQuery] string Description)
        {
            var query = new GetEftTransactionByParameterQuery(ReferenceNumber, TransactionDate, Amount, Description);
            var result = await _mediator.Send(query);
            return Ok(result); // ApiResponse<List<EftTransactionResponse>> direkt olarak döndürülüyor.
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EftTransactionResponse>>> CreateEftTransaction([FromBody] EftTransactionRequest EftTransaction)
        {
            var command = new CreateEftTransactionCommand(EftTransaction);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<EftTransactionResponse> direkt olarak döndürülüyor.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EftTransactionResponse>>> UpdateEftTransaction(int id, [FromBody] EftTransactionRequest EftTransaction)
        {
            var command = new UpdateAdressCommand(id, EftTransaction);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse<EftTransactionResponse> direkt olarak döndürülüyor.
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteEftTransaction(int id)
        {
            var command = new DeleteEftTransactionCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result); // ApiResponse direkt olarak döndürülüyor.
        }
    }
}
