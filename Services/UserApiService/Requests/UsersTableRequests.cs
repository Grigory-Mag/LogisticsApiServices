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
* --- USERS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<ListLogins> GetListUsers(Empty request, ServerCallContext context)
        {
            var users = dbContext.Users
                .Include(rn => rn.RoleNavigation)
                .ToList();
            var listUsers = new ListLogins();
            users.ForEach(x => x.Password = "<Установлен.Введите пароль, чтобы перезаписать его>");
            users.ForEach(item => listUsers.Logins.Add((LoginObject)item));
            if (listUsers.Logins.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Users not found"));

            return await Task.FromResult(listUsers);
        }

        [Authorize]
        public override async Task<LoginObject> UpdateUser(LoginRequest request, ServerCallContext context)
        {
            if (request.Data == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            var trackedEntity = dbContext.Users.First(x => x.Id == request.Data.Id);
            if (request.Data.Password == "Don't set")
                request.Data.Password = trackedEntity.Password;

            trackedEntity.Name = request.Data.Name;
            trackedEntity.Surname = request.Data.Surname;
            trackedEntity.Patronymic = request.Data.Patronymic;
            trackedEntity.Role = request.Data.UserRole.Id;
            trackedEntity.Password = request.Data.Password;
            trackedEntity.Login = request.Data.Login;

            //dbContext.Users.Update((User)request.Data);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Data);
        }

        [Authorize]
        public override async Task<LoginObject> CreateUser(LoginRequest request, ServerCallContext context)
        {
            var reply = request.Data;
            var user = (User)request.Data;
            user.Role = user.RoleNavigation!.Id;
            user.RoleNavigation = null;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            reply.Id = user.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<LoginObject> DeleteUser(LoginRequest request, ServerCallContext context)
        {
            var user = await dbContext.Users.FindAsync(request.Data.Id);
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((LoginObject)user);
        }
    }
}
