using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- OWNERSHIP TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<OwnershipsObject> GetOwnership(GetOrDeleteOwnershipsRequest request, ServerCallContext context)
        {
            var item = await dbContext.Ownerships.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));

            return await Task.FromResult((OwnershipsObject)item);
        }

        public override async Task<ListOwnerships> GetListOwnerships(Empty request, ServerCallContext context)
        {
            var listItems = new ListOwnerships();
            var items = dbContext.Ownerships.Select(item =>
                new OwnershipsObject((OwnershipsObject)item)
            ).ToList();
            listItems.Ownership.AddRange(items);
            if (listItems.Ownership.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Ownerships not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<OwnershipsObject> CreateOwnership(CreateOrUpdateOwnershipsRequest request, ServerCallContext context)
        {
            var item = (Ownership)request.Ownership;
            await dbContext.Ownerships.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((OwnershipsObject)item);
        }

        public override async Task<OwnershipsObject> UpdateOwnership(CreateOrUpdateOwnershipsRequest request, ServerCallContext context)
        {
            var item = await dbContext.Ownerships.FindAsync(request.Ownership.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));
            item = (Ownership)request.Ownership;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Ownership);
        }

        public override async Task<OwnershipsObject> DeleteOwnership(GetOrDeleteOwnershipsRequest request, ServerCallContext context)
        {
            var item = await dbContext.Ownerships.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));
            dbContext.Ownerships.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((OwnershipsObject)item);
        }

    }
}
