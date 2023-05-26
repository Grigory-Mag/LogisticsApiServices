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
* --- USER_ROLES TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<ListUserRoles> GetListUserRoles(Empty request, ServerCallContext context)
        {
            var item = dbContext.UserRoles.ToList();
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Roles not found"));

            var listItems = new ListUserRoles();
            item.ForEach(x => listItems.UserRole.Add((UserRoleObject)x));

            return await Task.FromResult(listItems);
        }

        [Authorize]
        public override async Task<UserRoleObject> CreateUserRole(CreateOrUpdateUserRoleRequest request, ServerCallContext context)
        {
            var reply = request.UserRole;
            var item = (UserRole)request.UserRole;

            await dbContext.UserRoles.AddAsync(item);
            await dbContext.SaveChangesAsync();

            reply.Id = item.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<UserRoleObject> UpdateUserRole(CreateOrUpdateUserRoleRequest request, ServerCallContext context)
        {
            if (request.UserRole == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
            dbContext.UserRoles.Update((UserRole)request.UserRole);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.UserRole);
        }

        [Authorize]
        public override async Task<UserRoleObject> DeleteUserRole(GetOrDeleteUserRoleRequest request, ServerCallContext context)
        {
            var item = await dbContext.UserRoles.FindAsync(request.Id);
            if (item == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
            dbContext.UserRoles.Remove(item);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((UserRoleObject)item);
        }
    }
}
