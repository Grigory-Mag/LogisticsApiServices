﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LogisticsApiServices.DBPostModels;

namespace ApiService
{
    public partial class UserApiService
    {

        /*
     * =*=*=*=*=*=*=*=*=*=*=*=*=*
     * CRUD OPERATIONS FOR 
     * --- CARGO TYPES TABLE ---
     * =*=*=*=*=*=*=*=*=*=*=*=*=*
     */

        public override async Task<CargoTypesObject> GetCargoType(GetOrDeleteCargoTypesRequest request, ServerCallContext context)
        {
            var cargoType = await dbContext.CargoTypes.FindAsync(request.Id);
            if (cargoType == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
            var cargoTypeObject = (CargoTypesObject)cargoType;

            return await Task.FromResult(cargoTypeObject);
        }

        public override async Task<ListCargoType> GetListCargoTypes(Empty request, ServerCallContext context)
        {
            var listCargoTypes = new ListCargoType();
            var cargoTypes = dbContext.CargoTypes.Select(item => new CargoTypesObject
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
            listCargoTypes.CargoType.AddRange(cargoTypes);
            if (listCargoTypes.CargoType.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoTypes not found"));

            return await Task.FromResult(listCargoTypes);
        }

        public override async Task<CargoTypesObject> CreateCargoType(CreateOrUpdateCargoTypesRequest request, ServerCallContext context)
        {
            var cargoTypeObject = request.CargoType;
            var cargoTypeDB = (CargoType)cargoTypeObject;
            await dbContext.CargoTypes.AddAsync(cargoTypeDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(cargoTypeObject);
        }

        public override async Task<CargoTypesObject> UpdateCargoType(CreateOrUpdateCargoTypesRequest request, ServerCallContext context)
        {
            var cargoTypeDB = await dbContext.CargoTypes.FindAsync(request.CargoType.Id);
            if (cargoTypeDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
            cargoTypeDB = (CargoType)request.CargoType;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.CargoType);
        }

        public override async Task<CargoTypesObject> DeleteCargoType(GetOrDeleteCargoTypesRequest request, ServerCallContext context)
        {
            var cargoTypeDB = await dbContext.CargoTypes.FindAsync(request.Id);
            if (cargoTypeDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
            dbContext.CargoTypes.Remove(cargoTypeDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((CargoTypesObject)cargoTypeDB);
        }
    }
}
