using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- TRANSPORTER TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<TransportersObject> GetTransporter(GetOrDeleteTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.Transporters.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Transporter not found"));

            return await Task.FromResult((TransportersObject)item);
        }

        public override async Task<ListTransporters> GetListTransporter(Empty request, ServerCallContext context)
        {
            var listItems = new ListTransporters();
            var items = dbContext.Transporters.Select(item =>
                new TransportersObject((TransportersObject)item)
            ).ToList();
            listItems.Transporter.AddRange(items);
            if (listItems.Transporter.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Transporters not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<TransportersObject> CreateTransporter(CreateOrUpdateTransportersRequest request, ServerCallContext context)
        {
            var item = (Transporter)request.Transporter;
            await dbContext.Transporters.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((TransportersObject)item);
        }

        public override async Task<TransportersObject> UpdateTransporter(CreateOrUpdateTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.Transporters.FindAsync(request.Transporter.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Transporter not found"));
            item = (Transporter)request.Transporter;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Transporter);
        }

        public override async Task<TransportersObject> DeleteTransporter(GetOrDeleteTransportersRequest request, ServerCallContext context)
        {
            var item = await dbContext.Transporters.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Transporter not found"));
            dbContext.Transporters.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((TransportersObject)item);
        }
    }
}
