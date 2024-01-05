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
    public class CreateAddressCommandHandler :
        IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
        IRequestHandler<UpdateAdressCommand, ApiResponse>,
        IRequestHandler<DeleteAddressCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public CreateAddressCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AddressRequest, Address>(request.Model);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<Address, AddressResponse>(entityResult.Entity);
            return new ApiResponse<AddressResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAdressCommand request,
            CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Address>().Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            fromdb.Address1 = request.Model.Address1;
            fromdb.Address2 = request.Model.Address2;
            fromdb.Country = request.Model.Country;
            fromdb.City = request.Model.City;
            fromdb.County = request.Model.County;
            fromdb.PostalCode = request.Model.PostalCode;
            fromdb.IsDefault = request.Model.IsDefault;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAddressCommand request,
            CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Address>().Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record not found");
            }

            //dbContext.Set<Address>().Remove(fromdb);
            fromdb.IsActive = false; // Veya ba�ka bir i�aretleme y�ntemi kullan�labilir.

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
