using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- REQUISITE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<RequisitesObject> GetRequisite(GetOrDeleteRequisitesRequest request, ServerCallContext context)
        {
            var item = dbContext.Requisites
                .Include(item => item.TypeNavigation)
                .Include(item => item.RoleNavigation)
                .First(item => item.Id == request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult((RequisitesObject)item);
        }

        [Authorize]
        public override async Task<ListRequisites> GetListRequisites(Empty request, ServerCallContext context)
        {
            var items = dbContext.Requisites
                .Include(item => item.RoleNavigation)
                .Include(item => item.TypeNavigation)
                .ToList();
            var itemsReady = new List<RequisitesObject>();
            items.ForEach(val => itemsReady.Add((RequisitesObject)val));

            var listItems = new ListRequisites();
            listItems.Requisites.AddRange(itemsReady);
            if (listItems.Requisites.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult(listItems);
        }

        [Authorize]
        public override async Task<RequisitesObject> CreateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
        {
            var reply = request.Requisite;
            var item = (Requisite)request.Requisite;
            item.Role = item.RoleNavigation.Id;
            item.Type = item.TypeNavigation.Id;
            item.TypeNavigation = null;
            item.RoleNavigation = null;

            await dbContext.Requisites.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<RequisitesObject> UpdateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
        {
            //var item = await dbContext.Requisites.FindAsync(request.Requisite.Id);
            if (request.Requisite == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
            dbContext.Requisites.Update((Requisite)request.Requisite);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Requisite);
        }

        [Authorize]
        public override async Task<RequisitesObject> DeleteRequisite(GetOrDeleteRequisitesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Requisites.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
            dbContext.Requisites.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RequisitesObject)item);
        }
    }
}
