using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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
            var item = await dbContext.Requests.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));

            return await Task.FromResult((RequestsObject)item);
        }

        public override async Task<ListRequest> GetListRequests(Empty request, ServerCallContext context)
        {
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
            var item = (Request)request.Requests;
            await dbContext.Requests.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RequestsObject)item);
        }

        public override async Task<RequestsObject> UpdateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
        {
            var item = await dbContext.Requests.FindAsync(request.Requests.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
            item = (Request)request.Requests;
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
