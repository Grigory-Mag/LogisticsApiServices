using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

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
            var item = dbContext.Vehicles
                .Include(on => on.OwnerNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(tn => tn.TypeNavigation)
                 .First(i => i.Id == request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));

            return await Task.FromResult((VehiclesObject)item);
        }

        public override async Task<ListVehicles> GetListVehicles(Empty request, ServerCallContext context)
        {
            var items = dbContext.Vehicles
                .Include(on => on.OwnerNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(tn => tn.TypeNavigation)
                 .ToList();
            var itemsReady = new List<VehiclesObject>();
            items.ForEach(item => itemsReady.Add((VehiclesObject)item));

            var listItems = new ListVehicles();
            listItems.Vehicle.AddRange(itemsReady);
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
            //var item = await dbContext.Vehicles.FindAsync(request.Vehicle.Id);
            if (request.Vehicle == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
            dbContext.Vehicles.Update((Vehicle)request.Vehicle);
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
