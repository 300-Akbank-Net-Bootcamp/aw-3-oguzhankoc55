using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CustomerResponse>>>> GetAllCustomers()
        {
            var query = new GetAllCustomerQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerResponse>>> GetCustomerById(int id)
        {
            var query = new GetCustomerByIdQuery(id);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CustomerResponse>>> CreateCustomer([FromBody] CustomerRequest customer)
        {
            var command = new CreateCustomerCommand(customer);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomer(int id, [FromBody] CustomerRequest customer)
        {
            var command = new UpdateCustomerCommand(id, customer);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomer(int id)
        {
            var command = new DeleteCustomerCommand(id);
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
