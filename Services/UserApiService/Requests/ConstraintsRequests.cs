using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- CARGO CONSTRAINTS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        public override async Task<ConstraintsObject> GetConstraint(GetOrDeleteConstraintsRequest request, ServerCallContext context)
        {
            var constraint = await dbContext.Constraints.FindAsync(request.Id);
            if (constraint == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Constraint not found"));
            var constraintObject = (ConstraintsObject)constraint;

            return await Task.FromResult(constraintObject);
        }

        public override async Task<ListConstraints> GetListConstraints(Empty request, ServerCallContext context)
        {
            var listConstraints = new ListConstraints();
            var cargoConstraints = dbContext.Constraints.Select(item => new ConstraintsObject
            {
                Id = item.Id,
                Desc = item.Desc
            }).ToList();
            listConstraints.Constraints.AddRange(cargoConstraints);
            if (listConstraints.Constraints.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "CargoTypes not found"));

            return await Task.FromResult(listConstraints);
        }

        public override async Task<ConstraintsObject> CreateConstraint(CreateOrUpdateConstraintsRequest request, ServerCallContext context)
        {
            var constraintObject = request.Constraint;
            var constraintDB = (Constraint)constraintObject;
            await dbContext.Constraints.AddAsync(constraintDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(constraintObject);
        }

        public override async Task<ConstraintsObject> UpdateConstraints(CreateOrUpdateConstraintsRequest request, ServerCallContext context)
        {
            var constraint = await dbContext.Constraints.FindAsync(request.Constraint.Id);
            if (constraint == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Constraint not found"));
            constraint = (Constraint)request.Constraint;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Constraint);
        }

        public override async Task<ConstraintsObject> DeleteConstraint(GetOrDeleteConstraintsRequest request, ServerCallContext context)
        {
            var constraint = await dbContext.Constraints.FindAsync(request.Id);
            if (constraint == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Constraint not found"));
            dbContext.Constraints.Remove(constraint);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((ConstraintsObject)constraint);
        }
    }
}
