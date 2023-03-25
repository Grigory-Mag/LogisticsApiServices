using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- TRANSPORTERS VEHICLE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<VehiclesTransportersObject> GetVehiclesTransporter(GetOrDeleteVehiclesTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehiclesTransporters.FindAsync(request.IdTransporter, request.IdVehicle);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Transporters not found"));

            return await Task.FromResult((VehiclesTransportersObject)item);

        }

        public override async Task<ListVehiclesTransporters> GetListVehiclesTransporter(Empty request, ServerCallContext context)
        {
            var listItems = new ListVehiclesTransporters();
            var items = dbContext.VehiclesTransporters.Select(item =>
                new VehiclesTransportersObject((VehiclesTransportersObject)item)
            ).ToList();
            listItems.VehicleTransporters.AddRange(items);
            if (listItems.VehicleTransporters.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Transporters not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<VehiclesTransportersObject> CreateVehiclesTransporter(CreateOrUpdateVehiclesTransportersRequest request, ServerCallContext context)
        {
            var item = (VehiclesTransporter)request.VehicleTransporters;
            await dbContext.VehiclesTransporters.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesTransportersObject)item);
        }

        public override async Task<VehiclesTransportersObject> UpdateVehiclesTransporter(CreateOrUpdateVehiclesTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehiclesTransporters.FindAsync(request.VehicleTransporters.Vehicle, request.VehicleTransporters.Vehicle);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "VehicleTransporter not found"));
            item = (VehiclesTransporter)request.VehicleTransporters;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.VehicleTransporters);
        }

        public override async Task<VehiclesTransportersObject> DeleteVehiclesTransporter(GetOrDeleteVehiclesTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.VehiclesTransporters.FindAsync(request.IdTransporter, request.IdVehicle);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
            dbContext.VehiclesTransporters.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((VehiclesTransportersObject)item);
        }
    }
}
