using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Cryptography.Xml;

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

        [Authorize]
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
                 .Include (rn => rn.IdRoutes)
                    .ThenInclude(an => an.ActionNavigation)
                .Where(i => i.Id == request.Id)
                .First();

            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));

            return await Task.FromResult((RequestsObject)item);
        }

        [Authorize]
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
                 .Include(rn => rn.IdRoutes)
                    .ThenInclude(an => an.ActionNavigation)
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

        [Authorize]
        public override async Task<RequestsObject> CreateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
        {
            var reply = request.Requests;
            var item = (Request)request.Requests;
            var idRoutes = item.IdRoutes.ToList();
            idRoutes.ForEach(route =>
            {
                route.Action = route.ActionNavigation.Id;
                route.ActionNavigation = null;
            });
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
            item.IdRoutes = idRoutes;
            

            await dbContext.Requests.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<RequestsObject> UpdateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
        {
            var item = (Request)request.Requests;
            var reply = (Request)request.Requests;
            var idRoutes = item.IdRoutes.ToList();
            idRoutes.ForEach(route =>
            {
                route.Action = route.ActionNavigation.Id;
                route.ActionNavigation = null;
            });
            item.Transporter = item.TransporterNavigation!.Id;
            item.Customer = item.CustomerNavigation!.Id;
            item.Cargo = item.CargoNavigation!.Id;
            item.Driver = item.DriverNavigation!.Id;
            item.Vehicle = item.VehicleNavigation!.Id;

            item.TransporterNavigation = null;
            item.CustomerNavigation = null;
            item.CargoNavigation = null;
            item.DriverNavigation = null;
            item.VehicleNavigation = null;
            item.IdRoutes = new List<LogisticsApiServices.DBPostModels.Route>();


            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
            var trackedItem = dbContext.Requests.Where(x => x.Id == item.Id).Include(rn => rn.IdRoutes).First();
            trackedItem.Transporter = item.Transporter;
            trackedItem.Customer = item.Customer;
            trackedItem.Cargo = item.Cargo;
            trackedItem.Driver = item.Driver;
            trackedItem.Vehicle = item.Vehicle;
            trackedItem.Price = item.Price;
            trackedItem.CreationDate = item.CreationDate;
            trackedItem.IsFinishied = item.IsFinishied;
            trackedItem.DocumentsOriginal = item.DocumentsOriginal;
            trackedItem.IdRoutes.Clear();
            
            //dbContext.Requests.Update(trackedItem);
            //dbContext.Routes.UpdateRange(idRoutes);
            await dbContext.SaveChangesAsync();
            dbContext = new DBContext();
            trackedItem = dbContext.Requests.Where(x => x.Id == item.Id).Include(rn => rn.IdRoutes).First();
            trackedItem.IdRoutes = idRoutes;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Requests);
        }

        [Authorize]
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
