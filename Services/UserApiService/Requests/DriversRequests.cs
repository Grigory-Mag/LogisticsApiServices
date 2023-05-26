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
* --- DRIVERS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

        [Authorize]
        public override async Task<DriversObject> GetDriver(GetOrDeleteDriversRequest request, ServerCallContext context)
        {
            var driver = dbContext.Drivers
                .Include(ln => ln.LicenceNavigation)
                .First(i => i.Id == request.Id);
            if (driver == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver not found"));

            return await Task.FromResult((DriversObject)driver);
        }

        [Authorize]
        public override async Task<ListDrivers> GetListDrivers(Empty request, ServerCallContext context)
        {
            var drivers = dbContext.Drivers
            .Include(ln => ln.LicenceNavigation)
            .ToList();
            var listDrivers = new ListDrivers();
            drivers.ForEach(item => listDrivers.Drivers.Add((DriversObject)item));
            if (listDrivers.Drivers.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Drivers not found"));

            return await Task.FromResult(listDrivers);
        }

        [Authorize]
        public override async Task<DriversObject> CreateDriver(CreateOrUpdateDriversRequest request, ServerCallContext context)
        {
            var reply = request.Driver;
            var driver = (Driver)request.Driver;
            driver.Licence = driver.LicenceNavigation.Id;
            driver.LicenceNavigation = null;

            await dbContext.Drivers.AddAsync(driver);
            await dbContext.SaveChangesAsync();

            reply.Id = driver.Id;
            return await Task.FromResult(reply);
        }

        [Authorize]
        public override async Task<DriversObject> UpdateDriver(CreateOrUpdateDriversRequest request, ServerCallContext context)
        {
            //var driver = await dbContext.Drivers.FindAsync(request.Driver.Id);
            if (request.Driver == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver not found"));
            dbContext.Drivers.Update((Driver)request.Driver);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Driver);
        }

        [Authorize]
        public override async Task<DriversObject> DeleteDriver(GetOrDeleteDriversRequest request, ServerCallContext context)
        {
            var driver = await dbContext.Drivers.FindAsync(request.Id);
            if (driver == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver not found"));
            dbContext.Drivers.Remove(driver);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((DriversObject)driver);
        }
    }
}
