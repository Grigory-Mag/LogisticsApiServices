using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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

        public override async Task<DriversObject> GetDriver(GetOrDeleteDriversRequest request, ServerCallContext context)
        {
            var driver = await dbContext.Drivers.FindAsync(request.Id);
            if (driver == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver not found"));

            return await Task.FromResult((DriversObject)driver);
        }

        public override async Task<ListDrivers> GetListDrivers(Empty request, ServerCallContext context)
        {
            var listDrivers = new ListDrivers();
            var drivers = dbContext.Drivers.Select(item =>
                new DriversObject((DriversObject)item)
            ).ToList();
            listDrivers.Drivers.AddRange(drivers);
            if (listDrivers.Drivers.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "Drivers not found"));

            return await Task.FromResult(listDrivers);
        }

        public override async Task<DriversObject> CreateDriver(CreateOrUpdateDriversRequest request, ServerCallContext context)
        {
            var driver = (Driver)request.Driver;
            await dbContext.Drivers.AddAsync(driver);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult((DriversObject)driver);
        }

        public override async Task<DriversObject> UpdateDriver(CreateOrUpdateDriversRequest request, ServerCallContext context)
        {
            var driver = await dbContext.Drivers.FindAsync(request.Driver.Id);
            if (driver == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Driver not found"));
            driver = (Driver)request.Driver;
            await dbContext.SaveChangesAsync();

            return await Task.FromResult(request.Driver);
        }

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
