using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Command
{
    public class CreateAccountTransactionCommandHandler :
        IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
        IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
        IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public CreateAccountTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entityResult.Entity);
            return new ApiResponse<AccountTransactionResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            fromdb.Amount = request.Model.Amount;
            fromdb.Description = request.Model.Description;
            fromdb.TransactionDate = request.Model.TransactionDate;
            fromdb.TransferType = request.Model.TransferType;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            //dbContext.Set<AccountTransaction>().Remove(fromdb);
            fromdb.IsActive = false; // Veya ba�ka bir i�aretleme y�ntemi kullan�labilir.

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
