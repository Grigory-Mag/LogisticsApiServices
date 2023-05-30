using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LogisticsApiServices.DBPostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.InteropServices;

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

        /// <summary>
        /// Implementation of a get function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>
        [Authorize]
        public override async Task<CargoObject> GetCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
        {
            var cargo = dbContext.Cargos
            .Include(i => i.TypeNavigation)
            .Where(item => item.Id == request.Id).First();

            if (cargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            var cargoObject = (CargoObject)cargo;
            return await Task.FromResult(cargoObject);
        }

        /// <summary>
        /// Implementation of a getList function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a list of rows in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>
        [Authorize]
        public override async Task<ListCargo> GetListCargo(Empty request, ServerCallContext context)
        {
            var data = dbContext.Cargos
            .Include(i => i.TypeNavigation).ToList();

            var dataReady = new List<CargoObject>();
            data.ForEach(item => dataReady.Add((CargoObject)item));

            var listCargoObjects = new ListCargo();
            listCargoObjects.Cargo.AddRange(dataReady);

            /*var listCargoObjects = new ListCargo();
            var cargos = dbContext.Cargos.Select(item => new CargoObject
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type,
                Volume = item.Volume,
                Weight = item.Volume
            }).ToList();
            listCargoObjects.Cargo.AddRange(cargos);*/
            if (listCargoObjects.Cargo.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            return await Task.FromResult(listCargoObjects);
        }

        /// <summary>
        /// Implementation of a create function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>
        [Authorize]
        public override async Task<CargoObject> CreateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
        {
            var reply = request.Cargo;
            var cargo = (Cargo)request.Cargo;
            cargo.Type = cargo.TypeNavigation.Id;
            cargo.TypeNavigation = null;

            await dbContext.Cargos.AddAsync(cargo);
            await dbContext.SaveChangesAsync();

            reply.Id = cargo.Id;
            return await Task.FromResult(reply);
        }

        /// <summary>
        /// Implementation of an update function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>
        [Authorize]
        public override async Task<CargoObject> UpdateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
        {
            var cargo = (Cargo)request.Cargo;
            if (cargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            dbContext.Cargos.Update(cargo);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Cargo);
        }

        /// <summary>
        /// Implementation of a delete function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>

        [Authorize]
        public override async Task<CargoObject> DeleteCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
        {
            var cargoDB = await dbContext.Cargos.FindAsync(request.Id);
            var cargoObject = (CargoObject)cargoDB;
            if (cargoDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
            dbContext.Cargos.Remove(cargoDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(cargoObject);
        }

    }
}
