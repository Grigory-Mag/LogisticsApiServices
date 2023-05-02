using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LogisticsApiServices.DBPostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.InteropServices;

namespace ApiService
{
    public partial class UserApiService
    {/*
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        * CRUD OPERATIONS FOR 
        * --- CARGO TABLE ---
        * =*=*=*=*=*=*=*=*=*=*=*=*=*
        */
        [Authorize]
        public override async Task<RolesObject> GetRole(GetOrDeleteRoleRequest request, ServerCallContext context)
        {
            var role = dbContext.Roles.First();

            if (role == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
            return await Task.FromResult((RolesObject)role);
        }

        public override async Task<ListRoles> GetListRoles(Empty request, ServerCallContext context)
        {
            var data = dbContext.Roles.ToList();

            var dataReady = new List<RolesObject>();
            data.ForEach(item => dataReady.Add((RolesObject)item));

            var listCargoObjects = new ListRoles();
            listCargoObjects.RolesObject.AddRange(dataReady);

            if (listCargoObjects.RolesObject.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Roles not found"));
            return await Task.FromResult(listCargoObjects);
        }

        public override async Task<RolesObject> CreateRole(CreateOrUpdateRoleRequest request, ServerCallContext context)
        {
            var reply = request.RoleObject;
            var role = (Role)request.RoleObject;

            await dbContext.Roles.AddAsync(role);
            await dbContext.SaveChangesAsync();

            reply.Id = role.Id;
            return await Task.FromResult(reply);
        }

        public override async Task<RolesObject> UpdateRole(CreateOrUpdateRoleRequest request, ServerCallContext context)
        {
            var role = (Role)request.RoleObject;
            if (role == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.RoleObject);
        }

        /// <summary>
        /// Implementation of a delete function for cargo table, declared in crud.proto file.
        /// </summary>
        /// <param name="request">Requested id</param>
        /// <param name="context"></param>
        /// <returns>Returns a row of a table in .proto class</returns>
        /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>

        public override async Task<RolesObject> DeleteRole (GetOrDeleteRoleRequest request, ServerCallContext context)
        {
            var roleDB = await dbContext.Roles.FindAsync(request.Id);
            var rolesObject = (RolesObject)roleDB;
            if (roleDB == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
            dbContext.Roles.Remove(roleDB);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(rolesObject);
        }

    }
}