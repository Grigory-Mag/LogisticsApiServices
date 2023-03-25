using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter;
using ApiService;
using Task = System.Threading.Tasks.Task;
using LogisticsApiServices.DBPostModels;

namespace ApiService;
public class UserApiService : UserService.UserServiceBase
{
    DBContext dbContext;
    public UserApiService(DBContext db)
    {
        this.dbContext = db;
    }

    /*
 * =*=*=*=*=*=*=*=*=*=*=*=*=*
 * CRUD OPERATIONS FOR 
 * --- CARGO TABLE ---
 * =*=*=*=*=*=*=*=*=*=*=*=*=*
 */

    public override async Task<CargoObject> GetCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
    {
        var cargo = await dbContext.Cargos.FindAsync(request.Id);
        if (cargo == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
        var cargoObject = (CargoObject)cargo;
        return await Task.FromResult(cargoObject);
        
        /* var user = new NewTestTable { StringData = request.Name, IntData = request.Age };
        await db.NewTestTables.AddAsync(user);
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(cargo); */
    }

    public override async Task<ListCargo> GetListCargo(Empty request, ServerCallContext context)
    {
        var listCargoObjects = new ListCargo();
        var cargos = dbContext.Cargos.Select(item => new CargoObject
        {
            Id = item.Id,
            Constraints = item.Constraints,
            Name = item.Name,
            Price = item.Price,
            Type = item.Type,
            Volume = item.Volume,
            Weight = item.Volume
        }).ToList();
        listCargoObjects.Cargo.AddRange(cargos);
        if (listCargoObjects.Cargo.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
        return await Task.FromResult(listCargoObjects);
    }

    public override async Task<CargoObject> CreateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
    {
        var cargoObject = request.Cargo;
        var cargo = (Cargo)cargoObject;

        await dbContext.Cargos.AddAsync(cargo);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(cargoObject);
    }

    public override async Task<CargoObject> UpdateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
    {
        var cargoObject = request.Cargo;
        var cargoDB = await dbContext.Cargos.FindAsync(request.Cargo.Id);
        if (cargoDB == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
        cargoDB.Type = cargoObject.Type;
        cargoDB.Constraints = cargoObject.Constraints;
        cargoDB.Weight = cargoObject.Weight;
        cargoDB.Volume = cargoObject.Volume;
        cargoDB.Name = cargoObject.Name;
        cargoDB.Price = cargoObject.Price;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(cargoObject);
    }

    /// <summary>
    /// Implementation of a delete function for cargo table, declared in crud.proto file.
    /// </summary>
    /// <param name="request">Requested id</param>
    /// <param name="context"></param>
    /// <returns>Returns a row of a table in .proto class</returns>
    /// <exception cref="RpcException">Throw exception when no data was found with requested id</exception>

    public override async Task<CargoObject> DeleteCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
    {
        var cargoDB = await dbContext.Cargos.FindAsync(request.Id);
        if (cargoDB == null)        
            throw new RpcException(new Status(StatusCode.NotFound, "Cargo not found"));
        dbContext.Cargos.Remove(cargoDB);
        await dbContext.SaveChangesAsync();
        var cargoObject = (CargoObject)cargoDB;

        return await Task.FromResult(cargoObject);
    }

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- CARGO CONSTRAINTS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<CargoConstraintsObject> GetCargoConstraint(GetOrDeleteCargoConstraintsRequest request, ServerCallContext context)
    {
        var cargoConstraint = await dbContext.CargoConstraints.FindAsync(request.IdCargo, request.IdConstraint);
        if (cargoConstraint == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraint not found"));
        var cargoConstraintObject = (CargoConstraintsObject)cargoConstraint;

        return await Task.FromResult(cargoConstraintObject);
    }

    public override async Task<ListCargoConstraints> GetListCargoConstraints(Empty request, ServerCallContext context)
    {
        var listCargoConstraints = new ListCargoConstraints();
        var cargoConstraints = dbContext.CargoConstraints.Select(item => new CargoConstraintsObject
        {
            IdCargo = item.IdCargo,
            IdConstraint = item.IdConstraint
        }).ToList();
        listCargoConstraints.CargoConstraints.AddRange(cargoConstraints);
        if (listCargoConstraints.CargoConstraints.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraints not found"));

        return await Task.FromResult(listCargoConstraints);
    }

    public override async Task<CargoConstraintsObject> CreateCargoConstraint(CreateOrUpdateCargoConstraintsRequest request, ServerCallContext context)
    {
        var cargoConstraintObject = request.CargoConstraints;
        var cargoConstraintDB = (CargoConstraint)cargoConstraintObject;
        await dbContext.CargoConstraints.AddAsync(cargoConstraintDB);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(cargoConstraintObject);
    }

    public override async Task<CargoConstraintsObject> UpdateCargoConstraint(CreateOrUpdateCargoConstraintsRequest request, ServerCallContext context)
    {
        var cargoConstraintDB = await dbContext.CargoConstraints.FindAsync(request.CargoConstraints.IdCargo, request.CargoConstraints.IdConstraint);
        if (cargoConstraintDB == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoConstraint not found"));
        cargoConstraintDB = (CargoConstraint)request.CargoConstraints;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.CargoConstraints);
    }

    public override async Task<CargoConstraintsObject> DeleteCargoConstraint(GetOrDeleteCargoConstraintsRequest request, ServerCallContext context)
    {
        var cargoConstraintDB = await dbContext.CargoConstraints.FindAsync(request.IdCargo, request.IdCargo);
        if (cargoConstraintDB == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
        dbContext.CargoConstraints.Remove(cargoConstraintDB);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((CargoConstraintsObject)cargoConstraintDB);
    }


    /*
 * =*=*=*=*=*=*=*=*=*=*=*=*=*
 * CRUD OPERATIONS FOR 
 * --- CARGO TYPES TABLE ---
 * =*=*=*=*=*=*=*=*=*=*=*=*=*
 */

    public override async Task<CargoTypesObject> GetCargoType(GetOrDeleteCargoTypesRequest request, ServerCallContext context)
    {
        var cargoType = await dbContext.CargoTypes.FindAsync(request.Id);
        if (cargoType == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
        var cargoTypeObject = (CargoTypesObject) cargoType;

        return await Task.FromResult(cargoTypeObject);
    }

    public override async Task<ListCargoType> GetListCargoTypes(Empty request, ServerCallContext context)
    {
        var listCargoTypes = new ListCargoType();
        var cargoTypes = dbContext.Cargos.Select(item => new CargoTypesObject
        {
            Id = item.Id,
            Name = item.Name
        }).ToList();
        listCargoTypes.CargoType.AddRange(cargoTypes);
        if (listCargoTypes.CargoType.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoTypes not found"));

        return await Task.FromResult(listCargoTypes);
    }

    public override async Task<CargoTypesObject> CreateCargoType(CreateOrUpdateCargoTypesRequest request, ServerCallContext context)
    {
        var cargoTypeObject = request.CargoType;
        var cargoTypeDB = (CargoType) cargoTypeObject;
        await dbContext.CargoTypes.AddAsync(cargoTypeDB);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(cargoTypeObject);
    }

    public override async Task<CargoTypesObject> UpdateCargoType(CreateOrUpdateCargoTypesRequest request, ServerCallContext context)
    {
        var cargoTypeDB = await dbContext.CargoTypes.FindAsync(request.CargoType.Id);
        if (cargoTypeDB == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
        cargoTypeDB = (CargoType) request.CargoType;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.CargoType);
    }

    public override async Task<CargoTypesObject> DeleteCargoType(GetOrDeleteCargoTypesRequest request, ServerCallContext context)
    {
        var cargoTypeDB = await dbContext.CargoTypes.FindAsync(request.Id);
        if (cargoTypeDB == null)
            throw new RpcException(new Status(StatusCode.NotFound, "CargoType not found"));
        dbContext.CargoTypes.Remove(cargoTypeDB);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((CargoTypesObject)cargoTypeDB);
    }

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

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- CUSTOMERS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<CustomersObject> GetCustomer(GetOrDeleteCustomersRequest request, ServerCallContext context)
    {
        var customer = await dbContext.Customers.FindAsync(request.Id);
        if (customer == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Constraint not found"));

        return await Task.FromResult((CustomersObject) customer);
    }

    public override async Task<ListCustomers> GetListCustomers(Empty request, ServerCallContext context)
    {
        var listCustomers = new ListCustomers();
        var customers = dbContext.Customers.Select(item => new CustomersObject
        {
            Id = item.Id,
            Cargo = item.Cargo,
            Requisite = item.Requisite
        }).ToList();
        listCustomers.Customers.AddRange(customers);
        if (listCustomers.Customers.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Customers not found"));

        return await Task.FromResult(listCustomers);
    }

    public override async Task<CustomersObject> CreateCustomer(CreateOrUpdateCustomersRequest request, ServerCallContext context)
    { 
        var customer = (Customer)request.Customer;
        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((CustomersObject)customer);
    }

    public override async Task<CustomersObject> UpdateCustomer(CreateOrUpdateCustomersRequest request, ServerCallContext context)
    {
        var customer = await dbContext.Customers.FindAsync(request.Customer.Id);
        if (customer == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
        customer = (Customer)request.Customer;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.Customer);
    }

    public override async Task<CustomersObject> DeleteCustomer(GetOrDeleteCustomersRequest request, ServerCallContext context)
    {
        var customer = await dbContext.Customers.FindAsync(request.Id);
        if (customer == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((CustomersObject)customer);
    }

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

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- DRIVER LICENCE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<DriverLicenceObject> GetDriverLicence(GetOrDeleteDriverLicenceRequest request, ServerCallContext context)
    {
        var driverLicence = await dbContext.DriverLicences.FindAsync(request.Id);
        if (driverLicence == null)
            throw new RpcException(new Status(StatusCode.NotFound, "DriverLicence not found"));

        return await Task.FromResult((DriverLicenceObject)driverLicence);
    }

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

    public override async Task<DriverLicenceObject> CreateDriverLicence(CreateOrUpdateDriverLicenceRequest request, ServerCallContext context)
    {
        var driverLicence = (DriverLicence)request.DriverLicence;
        await dbContext.DriverLicences.AddAsync(driverLicence);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((DriverLicenceObject)driverLicence);
    }

    public override async Task<DriverLicenceObject> UpdateDriverLicence(CreateOrUpdateDriverLicenceRequest request, ServerCallContext context)
    {
        var driverLicence = await dbContext.DriverLicences.FindAsync(request.DriverLicence.Id);
        if (driverLicence == null)
            throw new RpcException(new Status(StatusCode.NotFound, "DriverLicence not found"));
        driverLicence = (DriverLicence)request.DriverLicence;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.DriverLicence);
    }

    public override async Task<DriverLicenceObject> DeleteDriverLicence(GetOrDeleteDriverLicenceRequest request, ServerCallContext context)
    {
        var driverLicence = await dbContext.DriverLicences.FindAsync(request.Id);
        if (driverLicence == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Driver Licence not found"));
        dbContext.DriverLicences.Remove(driverLicence);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((DriverLicenceObject)driverLicence);
    }

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

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- OWNERSHIP TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<OwnershipsObject> GetOwnership(GetOrDeleteOwnershipsRequest request, ServerCallContext context)
    {
        var item = await dbContext.Ownerships.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));

        return await Task.FromResult((OwnershipsObject)item);
    }

    public override async Task<ListOwnerships> GetListOwnerships(Empty request, ServerCallContext context)
    {
        var listItems = new ListOwnerships();
        var items = dbContext.Ownerships.Select(item =>
            new OwnershipsObject((OwnershipsObject)item)
        ).ToList();
        listItems.Ownership.AddRange(items);
        if (listItems.Ownership.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Ownerships not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<OwnershipsObject> CreateOwnership(CreateOrUpdateOwnershipsRequest request, ServerCallContext context)
    {
        var item = (Ownership)request.Ownership;
        await dbContext.Ownerships.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((OwnershipsObject)item);
    }

    public override async Task<OwnershipsObject> UpdateOwnership(CreateOrUpdateOwnershipsRequest request, ServerCallContext context)
    {
        var item = await dbContext.Ownerships.FindAsync(request.Ownership.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));
        item = (Ownership)request.Ownership;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.Ownership);
    }

    public override async Task<OwnershipsObject> DeleteOwnership(GetOrDeleteOwnershipsRequest request, ServerCallContext context)
    {
        var item = await dbContext.Ownerships.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ownership not found"));
        dbContext.Ownerships.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((OwnershipsObject)item);
    }

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- REQUESTS TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<RequestsObject> GetRequest(GetOrDeleteRequestObjRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requests.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));

        return await Task.FromResult((RequestsObject)item);
    }

    public override async Task<ListRequest> GetListRequests(Empty request, ServerCallContext context)
    {
        var listItems = new ListRequest();
        var items = dbContext.Requests.Select(item =>
            new RequestsObject((RequestsObject)item)
        ).ToList();
        listItems.Requests.AddRange(items);
        if (listItems.Requests.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Requests not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<RequestsObject> CreateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
    {
        var item = (Request)request.Requests;
        await dbContext.Requests.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((RequestsObject)item);
    }

    public override async Task<RequestsObject> UpdateRequest(CreateOrUpdateRequestObjRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requests.FindAsync(request.Requests.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
        item = (Request)request.Requests;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.Requests);
    }

    public override async Task<RequestsObject> DeleteRequest(GetOrDeleteRequestObjRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requests.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Request not found"));
        dbContext.Requests.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((RequestsObject)item);
    }

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- REQUISITE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<RequisitesObject> GetRequisite(GetOrDeleteRequisitesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requisites.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

        return await Task.FromResult((RequisitesObject)item);
    }

    public override async Task<ListRequisites> GetListRequisites(Empty request, ServerCallContext context)
    {
        var listItems = new ListRequisites();
        var items = dbContext.Requisites.Select(item =>
            new RequisitesObject((RequisitesObject)item)
        ).ToList();
        listItems.Requisites.AddRange(items);
        if (listItems.Requisites.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<RequisitesObject> CreateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
    {
        var item = (Requisite)request.Requisite;
        await dbContext.Requisites.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((RequisitesObject)item);
    }

    public override async Task<RequisitesObject> UpdateRequisite(CreateOrUpdateRequisitesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requisites.FindAsync(request.Requisite.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
        item = (Requisite)request.Requisite;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.Requisite);
    }

    public override async Task<RequisitesObject> DeleteRequisite(GetOrDeleteRequisitesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Requisites.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Requisite not found"));
        dbContext.Requisites.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((RequisitesObject)item);
    }

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

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- VEHICLE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<VehiclesObject> GetVehicle(GetOrDeleteVehiclesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Vehicles.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));

        return await Task.FromResult((VehiclesObject)item);
    }

    public override async Task<ListVehicles> GetListVehicles(Empty request, ServerCallContext context)
    {
        var listItems = new ListVehicles();
        var items = dbContext.Vehicles.Select(item =>
            new VehiclesObject((VehiclesObject)item)
        ).ToList();
        listItems.Vehicle.AddRange(items);
        if (listItems.Vehicle.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicles not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<VehiclesObject> CreateVehicle(CreateOrUpdateVehiclesRequest request, ServerCallContext context)
    {
        var item = (Vehicle)request.Vehicle;
        await dbContext.Vehicles.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesObject)item);
    }

    public override async Task<VehiclesObject> UpdateVehicle(CreateOrUpdateVehiclesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Vehicles.FindAsync(request.Vehicle.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
        item = (Vehicle)request.Vehicle;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.Vehicle);
    }

    public override async Task<VehiclesObject> DeleteVehicle(GetOrDeleteVehiclesRequest request, ServerCallContext context)
    {
        var item = await dbContext.Vehicles.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle not found"));
        dbContext.Vehicles.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesObject)item);
    }


    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- VEHICLE TYPE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<VehiclesTypesObject> GetVehiclesType(GetOrDeleteVehiclesTypesRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehicleTypes.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "VehicleType not found"));

        return await Task.FromResult((VehiclesTypesObject)item);
    }

    public override async Task<ListVehiclesTypes> GetListVehiclesTypes(Empty request, ServerCallContext context)
    {
        var listItems = new ListVehiclesTypes();
        var items = dbContext.VehicleTypes.Select(item =>
            new VehiclesTypesObject((VehiclesTypesObject)item)
        ).ToList();
        listItems.VehiclesTypes.AddRange(items);
        if (listItems.VehiclesTypes.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Types not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<VehiclesTypesObject> CreateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
    {
        var item = (VehicleType)request.VehiclesTypes;
        await dbContext.VehicleTypes.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesTypesObject)item);
    }

    public override async Task<VehiclesTypesObject> UpdateVehiclesType(CreateOrUpdateVehiclesTypesRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehicleTypes.FindAsync(request.VehiclesTypes.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
        item = (VehicleType)request.VehiclesTypes;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.VehiclesTypes);
    }

    public override async Task<VehiclesTypesObject> DeleteVehiclesType(GetOrDeleteVehiclesTypesRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehicleTypes.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
        dbContext.VehicleTypes.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesTypesObject)item);
    }

    /*
* =*=*=*=*=*=*=*=*=*=*=*=*=*
* CRUD OPERATIONS FOR 
* --- TRANSPORTERS VEHICLE TABLE ---
* =*=*=*=*=*=*=*=*=*=*=*=*=*
*/

    public override async Task<VehiclesTransportersObject> GetVehiclesTransporter(GetOrDeleteVehiclesTransportersRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehiclesTransporters.FindAsync(request.IdTransporter, request.IdVehicle);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Transporters not found"));

        return await Task.FromResult((VehiclesTransportersObject)item);

    }

    public override async Task<ListVehiclesTransporters> GetListVehiclesTransporter(Empty request, ServerCallContext context)
    {
        var listItems = new ListVehiclesTransporters();
        var items = dbContext.VehiclesTransporters.Select(item =>
            new VehiclesTransportersObject((VehiclesTransportersObject)item)
        ).ToList();
        listItems.VehicleTransporters.AddRange(items);
        if (listItems.VehicleTransporters.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicles Transporters not found"));

        return await Task.FromResult(listItems);
    }

    public override async Task<VehiclesTransportersObject> CreateVehiclesTransporter(CreateOrUpdateVehiclesTransportersRequest request, ServerCallContext context)
    {
        var item = (VehiclesTransporter)request.VehicleTransporters;
        await dbContext.VehiclesTransporters.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesTransportersObject)item);
    }

    public override async Task<VehiclesTransportersObject> UpdateVehiclesTransporter(CreateOrUpdateVehiclesTransportersRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehiclesTransporters.FindAsync(request.VehicleTransporters.Vehicle, request.VehicleTransporters.Vehicle);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "VehicleTransporter not found"));
        item = (VehiclesTransporter)request.VehicleTransporters;
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(request.VehicleTransporters);
    }

    public override async Task<VehiclesTransportersObject> DeleteVehiclesTransporter(GetOrDeleteVehiclesTransportersRequest request, ServerCallContext context)
    {
        var item = await dbContext.VehiclesTransporters.FindAsync(request.IdTransporter, request.IdVehicle);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vehicle Type not found"));
        dbContext.VehiclesTransporters.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult((VehiclesTransportersObject)item);
    }
}