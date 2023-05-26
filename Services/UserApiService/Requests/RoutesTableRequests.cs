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
* --- ROUTES TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<RouteObject> GetRoute(GetOrDeleteRouteObjectRequest request, ServerCallContext context)
        {
            var item = dbContext.Routes
            .Include(an => an.ActionNavigation)
            .First(x => x.Id == request.Id);

            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Route not found"));

            return await Task.FromResult((RouteObject)item);
        }

        [Authorize]
        public override async Task<ListRouteObjects> GetListRoute(Empty request, ServerCallContext context)
        {
            var item = dbContext.Routes
                .Include(an => an.ActionNavigation)
                .ToList();
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Routes not found"));

            var listItems = new ListRouteObjects();
            item.ForEach(x=> listItems.RouteObjects.Add((RouteObject)x));

            return await Task.FromResult(listItems);
        }

        [Authorize]
        public override async Task<RouteObject> CreateRoute(CreateOrUpdateRouteObjectRequest request, ServerCallContext context)
        {
            var reply = request.RouteObject;
            var item = (LogisticsApiServices.DBPostModels.Route)request.RouteObject;
            item.Action = item.ActionNavigation.Id;
            item.ActionNavigation = null;

            await dbContext.Routes.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<RouteObject> UpdateRoute(CreateOrUpdateRouteObjectRequest request, ServerCallContext context)
        {
            if (request.RouteObject == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Route not found"));
            dbContext.Routes.Update((LogisticsApiServices.DBPostModels.Route)request.RouteObject);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.RouteObject);
        }

        [Authorize]
        public override async Task<RouteObject> DeleteRoute(GetOrDeleteRouteObjectRequest request, ServerCallContext context)
        {
            var route = await dbContext.Routes.FindAsync(request.Id);
            if (route == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Route not found"));
            dbContext.Routes.Remove(route);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RouteObject)route);
        }
    }
}
