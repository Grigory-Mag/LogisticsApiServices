using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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

        public override async Task<VehiclesTypesObject> GetVehiclesType(GetOrDeleteVehiclesTypesRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehicleTypes.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "VehicleType not found"));

            return await Task.FromResult((VehiclesTypesObject)item);
        }

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

        public override async Task<VehiclesTypesObject> CreateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
        {
            var item = (VehicleType)request.VehiclesTypes;
            await dbContext.VehicleTypes.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesTypesObject)item);
        }

        public override async Task<VehiclesTypesObject> UpdateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehicleTypes.FindAsync(request.VehiclesTypes.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
            item = (VehicleType)request.VehiclesTypes;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.VehiclesTypes);
        }

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
