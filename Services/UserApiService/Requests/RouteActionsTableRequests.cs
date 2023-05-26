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
* --- ROUTE_ACTIONS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<RouteActionsObject> GetRouteAction(GetOrDeleteRouteActionsRequest request, ServerCallContext context)
        {
            var item = dbContext.RouteActions
                .First(x => x.Id == request.Id);

            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "RouteAction not found"));

            return await Task.FromResult((RouteActionsObject)item);
        }

        [Authorize]
        public override async Task<ListRouteActions> GetListRouteActions(Empty request, ServerCallContext context)
        {
            var item = dbContext.RouteActions.ToList();
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "RouteActions not found"));

            var listItems = new ListRouteActions();
            item.ForEach(x => listItems.RouteActionsObject.Add((RouteActionsObject)x));

            return await Task.FromResult(listItems);
        }

        [Authorize]
        public override async Task<RouteActionsObject> CreateRouteAction(CreateOrUpdateRouteActionsRequest request, ServerCallContext context)
        {
            var reply = request.RouteAction;
            var item = (RouteAction)request.RouteAction;

            await dbContext.RouteActions.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<RouteActionsObject> UpdateRouteAction(CreateOrUpdateRouteActionsRequest request, ServerCallContext context)
        {
            if (request.RouteAction == null)
                throw new RpcException(new Status(StatusCode.NotFound, "RouteAction not found"));
            dbContext.RouteActions.Update((RouteAction)request.RouteAction);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.RouteAction);
        }

        [Authorize]
        public override async Task<RouteActionsObject> DeleteRouteAction(GetOrDeleteRouteActionsRequest request, ServerCallContext context)
        {
            var route = await dbContext.RouteActions.FindAsync(request.Id);
            if (route == null)
                throw new RpcException(new Status(StatusCode.NotFound, "RouteAction not found"));
            dbContext.RouteActions.Remove(route);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RouteActionsObject)route);
        }

    }
}
