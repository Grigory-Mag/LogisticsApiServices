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
* --- REQUESTS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<RequestsObject> GetRequest(GetOrDeleteRequestObjRequest request, ServerCallContext context)
        {
            var item = dbContext.Requests
                .Include(cn => cn.CargoNavigation)
                    .ThenInclude(ct => ct.TypeNavigation)
                 .Include(cun => cun.CustomerNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(tn => tn.TransporterNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(vn => vn.VehicleNavigation)
                    .Include(vn => vn.VehicleNavigation)
                    .ThenInclude(on => on.OwnerNavigation)
                    .Include(vn => vn.VehicleNavigation)
                    .ThenInclude(tn => tn.TypeNavigation)
                .Include(dn => dn.DriverNavigation)
                    .ThenInclude(ln => ln.LicenceNavigation)
                .Where(i => i.Id == request.Id)
                .First();

            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));

            return await Task.FromResult((RequestsObject)item);
        }

        public override async Task<ListRequest> GetListRequests(Empty request, ServerCallContext context)
        {
            var item = dbContext.Requests
                .Include(cn => cn.CargoNavigation)
                    .ThenInclude(ct => ct.TypeNavigation)
                 .Include(cun => cun.CustomerNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(tn => tn.TransporterNavigation)
                    .ThenInclude(rn => rn.RoleNavigation)
                 .Include(vn => vn.VehicleNavigation)
                    .Include(vn => vn.VehicleNavigation)
                    .ThenInclude(on => on.OwnerNavigation)
                    .Include(vn => vn.VehicleNavigation)
                    .ThenInclude(tn => tn.TypeNavigation)
                 .Include(dn => dn.DriverNavigation)
                    .ThenInclude(ln => ln.LicenceNavigation)
                 .ToList();

            var listItems = new ListRequest();
            var items = dbContext.Requests.Select(item =>
                new RequestsObject((RequestsObject)item)
            ).ToList();
            listItems.Requests.AddRange(items);
            if (listItems.Requests.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Requests not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<RequestsObject> CreateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
        {
            var reply = request.Requests;
            var item = (Request)request.Requests;
            item.Transporter = item.TransporterNavigation.Id;
            item.Customer = item.CustomerNavigation.Id;
            item.Cargo = item.CargoNavigation.Id;
            item.Driver = item.DriverNavigation.Id;
            item.Vehicle = item.VehicleNavigation.Id;

            item.TransporterNavigation = null;
            item.CustomerNavigation = null;
            item.CargoNavigation = null;
            item.DriverNavigation = null;
            item.VehicleNavigation = null;
            

            await dbContext.Requests.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        public override async Task<RequestsObject> UpdateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
        {
            var item = (Request)request.Requests;
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
            dbContext.Requests.Update(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Requests);
        }

        public override async Task<RequestsObject> DeleteRequest(GetOrDeleteRequestObjRequest request, ServerCallContext context)
        {
            var item = await dbContext.Requests.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
            dbContext.Requests.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RequestsObject)item);
        }
    }
}
