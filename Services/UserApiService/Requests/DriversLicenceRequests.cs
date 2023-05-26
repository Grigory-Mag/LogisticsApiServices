using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace ApiService
{
    public partial class UserApiService
    {
        /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- DRIVER LICENCE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/
        [Authorize]
        public override async Task<DriverLicenceObject> GetDriverLicence(GetOrDeleteDriverLicenceRequest request, ServerCallContext context)
        {
            var driverLicence = await dbContext.DriverLicences.FindAsync(request.Id);
            if (driverLicence == null)
                throw new RpcException(new Status(StatusCode.NotFound, "DriverLicence not found"));

            return await Task.FromResult((DriverLicenceObject)driverLicence);
        }

        [Authorize]
        public override async Task<ListDriverLicence> GetListDriverLicences(Empty request, ServerCallContext context)
        {
            var listDriversLicence = new ListDriverLicence();
            var driversLicence = dbContext.DriverLicences.Select(item =>
                new DriverLicenceObject((DriverLicenceObject)item)
            ).ToList();
            listDriversLicence.DriverLicence.AddRange(driversLicence);
            if (listDriversLicence.DriverLicence.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "DriverLicences not found"));

            return await Task.FromResult(listDriversLicence);
        }

        [Authorize]
        public override async Task<DriverLicenceObject> CreateDriverLicence(CreateOrUpdateDriverLicenceRequest request, ServerCallContext context)
        {
            var reply = request.DriverLicence;
            var driverLicence = (DriverLicence)request.DriverLicence;
            await dbContext.DriverLicences.AddAsync(driverLicence);
            await dbContext.SaveChangesAsync();

            reply.Id = driverLicence.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<DriverLicenceObject> UpdateDriverLicence(CreateOrUpdateDriverLicenceRequest request, ServerCallContext context)
        {
            //var driverLicence = await dbContext.DriverLicences.FindAsync(request.DriverLicence.Id);
            if (request.DriverLicence == null)
                throw new RpcException(new Status(StatusCode.NotFound, "DriverLicence not found"));
            dbContext.DriverLicences.Update((DriverLicence)request.DriverLicence);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.DriverLicence);
        }

        [Authorize]
        public override async Task<DriverLicenceObject> DeleteDriverLicence(GetOrDeleteDriverLicenceRequest request, ServerCallContext context)
        {
            var driverLicence = await dbContext.DriverLicences.FindAsync(request.Id);      

            if (driverLicence == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver Licence not found"));
            dbContext.DriverLicences.Remove(driverLicence);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((DriverLicenceObject)driverLicence);
        }
    }
}
