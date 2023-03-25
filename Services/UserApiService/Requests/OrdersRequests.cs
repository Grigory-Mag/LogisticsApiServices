using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {

        /*
    * =*=*=*=*=*=*=*=*=*=*=*=*=*
    * CRUD OPERATIONS FOR 
    * --- ORDERS TABLE ---
    * =*=*=*=*=*=*=*=*=*=*=*=*=*
    */
        public override async Task<OrdersObject> GetOrder(GetOrDeleteOrdersRequest request, ServerCallContext context)
        {
            var orders = await dbContext.Orders.FindAsync(request.Id);
            if (orders == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));

            return await Task.FromResult((OrdersObject)orders);
        }

        public override async Task<ListOrders> GetListOrders(Empty request, ServerCallContext context)
        {
            var listOrders = new ListOrders();
            var orders = dbContext.Orders.Select(item =>
                new OrdersObject((OrdersObject)item)
            ).ToList();
            listOrders.Orders.AddRange(orders);
            if (listOrders.Orders.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "DriverLicences not found"));

            return await Task.FromResult(listOrders);
        }

        public override async Task<OrdersObject> CreateOrder(CreateOrUpdateOrdersRequest request, ServerCallContext context)
        {
            var order = (Order)request.Order;
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((OrdersObject)order);
        }

        public override async Task<OrdersObject> UpdateOrder(CreateOrUpdateOrdersRequest request, ServerCallContext context)
        {
            var order = await dbContext.Orders.FindAsync(request.Order.Id);
            if (order == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
            order = (Order)request.Order;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Order);
        }

        public override async Task<OrdersObject> DeleteOrder(GetOrDeleteOrdersRequest request, ServerCallContext context)
        {
            var order = await dbContext.Orders.FindAsync(request.Id);
            if (order == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((OrdersObject)order);
        }
    }
}
