using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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

        public override async Task<RequisitesObject> GetRequisite(GetOrDeleteRequisitesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Requisites.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult((RequisitesObject)item);
        }

        public override async Task<ListRequisites> GetListRequisites(Empty request, ServerCallContext context)
        {
            var listItems = new ListRequisites();
            var items = dbContext.Requisites.Select(item =>
                new RequisitesObject((RequisitesObject)item)
            ).ToList();
            listItems.Requisites.AddRange(items);
            if (listItems.Requisites.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<RequisitesObject> CreateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
        {
            var item = (Requisite)request.Requisite;
            await dbContext.Requisites.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RequisitesObject)item);
        }

        public override async Task<RequisitesObject> UpdateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
        {
            var item = await dbContext.Requisites.FindAsync(request.Requisite.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
            item = (Requisite)request.Requisite;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Requisite);
        }

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
