using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LogisticsApiServices.DBPostModels;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        * CRUD OPERATIONS FOR 
        * --- CARGO CONSTRAINTS TABLE ---
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        */

        public override async Task<CargoConstraintsObject> GetCargoConstraint(GetOrDeleteCargoConstraintsRequest request, ServerCallContext context)
        {
            var cargoConstraint = await dbContext.CargoConstraints.FindAsync(request.IdCargo, request.IdConstraint);
            if (cargoConstraint == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraint not found"));
            var cargoConstraintObject = (CargoConstraintsObject)cargoConstraint;

            return await Task.FromResult(cargoConstraintObject);
        }

        public override async Task<ListCargoConstraints> GetListCargoConstraints(Empty request, ServerCallContext context)
        {
            var listCargoConstraints = new ListCargoConstraints();
            var cargoConstraints = dbContext.CargoConstraints.Select(item => new CargoConstraintsObject
            {
                IdCargo = item.IdCargo,
                IdConstraint = item.IdConstraint
            }).ToList();
            listCargoConstraints.CargoConstraints.AddRange(cargoConstraints);
            if (listCargoConstraints.CargoConstraints.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraints not found"));

            return await Task.FromResult(listCargoConstraints);
        }

        public override async Task<CargoConstraintsObject> CreateCargoConstraint(CreateOrUpdateCargoConstraintsRequest request, ServerCallContext context)
        {
            var cargoConstraintObject = request.CargoConstraints;
            var cargoConstraintDB = (CargoConstraint)cargoConstraintObject;
            await dbContext.CargoConstraints.AddAsync(cargoConstraintDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(cargoConstraintObject);
        }

        public override async Task<CargoConstraintsObject> UpdateCargoConstraint(CreateOrUpdateCargoConstraintsRequest request, ServerCallContext context)
        {
            var cargoConstraintDB = await dbContext.CargoConstraints.FindAsync(request.CargoConstraints.IdCargo, request.CargoConstraints.IdConstraint);
            if (cargoConstraintDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraint not found"));
            cargoConstraintDB = (CargoConstraint)request.CargoConstraints;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.CargoConstraints);
        }

        public override async Task<CargoConstraintsObject> DeleteCargoConstraint(GetOrDeleteCargoConstraintsRequest request, ServerCallContext context)
        {
            var cargoConstraintDB = await dbContext.CargoConstraints.FindAsync(request.IdCargo, request.IdCargo);
            if (cargoConstraintDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
            dbContext.CargoConstraints.Remove(cargoConstraintDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((CargoConstraintsObject)cargoConstraintDB);
        }
    }
}
