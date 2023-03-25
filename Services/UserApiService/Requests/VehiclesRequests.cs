using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- VEHICLE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<VehiclesObject> GetVehicle(GetOrDeleteVehiclesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Vehicles.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));

            return await Task.FromResult((VehiclesObject)item);
        }

        public override async Task<ListVehicles> GetListVehicles(Empty request, ServerCallContext context)
        {
            var listItems = new ListVehicles();
            var items = dbContext.Vehicles.Select(item =>
                new VehiclesObject((VehiclesObject)item)
            ).ToList();
            listItems.Vehicle.AddRange(items);
            if (listItems.Vehicle.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicles not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<VehiclesObject> CreateVehicle(CreateOrUpdateVehiclesRequest request, ServerCallContext context)
        {
            var item = (Vehicle)request.Vehicle;
            await dbContext.Vehicles.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesObject)item);
        }

        public override async Task<VehiclesObject> UpdateVehicle(CreateOrUpdateVehiclesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Vehicles.FindAsync(request.Vehicle.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
            item = (Vehicle)request.Vehicle;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Vehicle);
        }

        public override async Task<VehiclesObject> DeleteVehicle(GetOrDeleteVehiclesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Vehicles.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
            dbContext.Vehicles.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesObject)item);
        }
    }
}
