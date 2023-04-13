using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LogisticsApiServices.DBPostModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Data.Entity;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        * CRUD OPERATIONS FOR 
        * --- CARGO TABLE ---
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        */
        [Authorize]
        public override async Task<CargoObject> GetCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
        {
            var cargo = await dbContext.Cargos.FindAsync(request.Id);
            if (cargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
            var cargoObject = (CargoObject)cargo;
            return await Task.FromResult(cargoObject);

            /* var user = new NewTestTable { StringData = request.Name, IntData = request.Age };
            await db.NewTestTables.AddAsync(user);
            await db.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
            return await Task.FromResult(cargo); */
        }

        public override async Task<ListCargo> GetListCargo(Empty request, ServerCallContext context)
        {
            var listCargoObjects = new ListCargo();
            var cargos = dbContext.Cargos.Select(item => new CargoObject
            {
                Id = item.Id,
                Constraints = item.Constraints,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type,
                Volume = item.Volume,
                Weight = item.Volume
            }).ToList();
            listCargoObjects.Cargo.AddRange(cargos);
            if (listCargoObjects.Cargo.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            return await Task.FromResult(listCargoObjects);
        }

        public override async Task<CargoObject> CreateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
        {
            var cargoObject = request.Cargo;
            var cargo = (Cargo)cargoObject;

            await dbContext.Cargos.AddAsync(cargo);
            await dbContext.SaveChangesAsync();
            return await Task.FromResult(cargoObject);
        }

        public override async Task<CargoObject> UpdateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
        {
            var cargoObject = request.Cargo;
            var cargoDB = await dbContext.Cargos.FindAsync(request.Cargo.Id);
            if (cargoDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            cargoDB.Type = cargoObject.Type;
            cargoDB.Constraints = cargoObject.Constraints;
            cargoDB.Weight = cargoObject.Weight;
            cargoDB.Volume = cargoObject.Volume;
            cargoDB.Name = cargoObject.Name;
            cargoDB.Price = cargoObject.Price;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(cargoObject);
        }

        /// <summary>
        /// Implementation of a delete function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>

        public override async Task<CargoObject> DeleteCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
        {
            var cargoDB = await dbContext.Cargos.FindAsync(request.Id);
            if (cargoDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            dbContext.Cargos.Remove(cargoDB);
            await dbContext.SaveChangesAsync();
            var cargoObject = (CargoObject)cargoDB;

            return await Task.FromResult(cargoObject);
        }

    }
}
