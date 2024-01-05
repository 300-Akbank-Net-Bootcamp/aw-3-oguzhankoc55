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
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
        IRequestHandler<UpdateAccountCommand, ApiResponse>,
        IRequestHandler<DeleteAccountCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public CreateAccountCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AccountRequest, Account>(request.Model);
            entity.AccountNumber = GenerateAccountNumber(); // Örnek bir account numarasý üretme metodu, ihtiyaca göre deðiþtirilebilir.

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<Account, AccountResponse>(entityResult.Entity);
            return new ApiResponse<AccountResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            fromdb.Balance = request.Model.Balance;
            fromdb.CurrencyType = request.Model.CurrencyType;
            fromdb.Name = request.Model.Name;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            //dbContext.Set<Account>().Remove(fromdb);
            fromdb.IsActive = false; // Veya baþka bir iþaretleme yöntemi kullanýlabilir.

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        private int GenerateAccountNumber()
        {
            // Ýhtiyaca göre özel bir account numarasý üretme metodu.
            // Bu örnek için sadece rasgele bir sayý döndürüyoruz.
            return new Random().Next(1000000, 9999999);
        }
    }
}
