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
* --- REQUISITE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<RequisiteTypeObject> GetRequisiteType(GetOrDeleteRequisiteTypeRequest request, ServerCallContext context)
        {
            var item = dbContext.RequisitesTypes
                .First(item => item.Id == request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult((RequisiteTypeObject)item);
        }

        public override async Task<ListRequisiteTypes> GetListRequisiteTypes(Empty request, ServerCallContext context)
        {
            var items = dbContext.RequisitesTypes
                .ToList();
            var itemsReady = new List<RequisiteTypeObject>();
            items.ForEach(val => itemsReady.Add((RequisiteTypeObject)val));

            var listItems = new ListRequisiteTypes();
            listItems.RequisiteType.AddRange(itemsReady);
            if (listItems.RequisiteType.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

            return await Task.FromResult(listItems);
        }

        public override async Task<RequisiteTypeObject> CreateRequisiteType(CreateOrUpdateRequisiteTypeRequest request, ServerCallContext context)
        {
            var reply = request.RequisiteType;
            var item = (RequisitesType)request.RequisiteType;

            await dbContext.RequisitesTypes.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        public override async Task<RequisiteTypeObject> UpdateRequisiteType(CreateOrUpdateRequisiteTypeRequest request, ServerCallContext context)
        {
            //var item = await dbContext.Requisites.FindAsync(request.Requisite.Id);
            if (request.RequisiteType == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
            dbContext.RequisitesTypes.Update((RequisitesType)request.RequisiteType);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.RequisiteType);
        }

        public override async Task<RequisiteTypeObject> DeleteRequisiteType(GetOrDeleteRequisiteTypeRequest request, ServerCallContext context)
        {
            var item = await dbContext.RequisitesTypes.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
            dbContext.RequisitesTypes.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((RequisiteTypeObject)item);
        }
    }
}
