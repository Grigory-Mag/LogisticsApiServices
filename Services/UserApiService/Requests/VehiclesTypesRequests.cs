using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- VEHICLE TYPE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<VehiclesTypesObject> GetVehiclesType(GetOrDeleteVehiclesTypesRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehicleTypes.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "VehicleType not found"));

            return await Task.FromResult((VehiclesTypesObject)item);
        }

        [Authorize]
        public override async Task<ListVehiclesTypes> GetListVehiclesTypes(Empty request, ServerCallContext context)
        {
            var listItems = new ListVehiclesTypes();
            var items = dbContext.VehicleTypes.Select(item =>
                new VehiclesTypesObject((VehiclesTypesObject)item)
            ).ToList();
            listItems.VehiclesTypes.AddRange(items);
            if (listItems.VehiclesTypes.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Types not found"));

            return await Task.FromResult(listItems);
        }

        [Authorize]
        public override async Task<VehiclesTypesObject> CreateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
        {
            var reply = request.VehiclesTypes;
            var item = (VehicleType)request.VehiclesTypes;
            await dbContext.VehicleTypes.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<VehiclesTypesObject> UpdateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
        {            
            if (request.VehiclesTypes == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
            dbContext.VehicleTypes.Update((VehicleType)request.VehiclesTypes);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.VehiclesTypes);
        }

        [Authorize]
        public override async Task<VehiclesTypesObject> DeleteVehiclesType(GetOrDeleteVehiclesTypesRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehicleTypes.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
            dbContext.VehicleTypes.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesTypesObject)item);
        }
    }
}
