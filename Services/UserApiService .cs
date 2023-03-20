using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter;
using ApiService;
using Task = System.Threading.Tasks.Task;
using GrpcGreeter.DBPostModels;

namespace ApiService;
public class UserApiService : UserService.UserServiceBase
{
    DBContext dbContext;
    public UserApiService(DBContext db)
    {
        this.dbContext = db;
    }

    private CargoObject CargoToCargoObject(Cargo cargo)
    {
        return new CargoObject()
        {
            Id = cargo.Id,
            Type = cargo.Type,
            Constraints = cargo.Constraints,
            Weight = cargo.Weight,
            Volume = cargo.Volume,
            Name = cargo.Name,
            Price = cargo.Price,
        };
    }

    private Cargo CargoObjectToCargo(CargoObject cargoObject)
    {
        return new Cargo()
        {
            Id = cargoObject.Id,
            Type = cargoObject.Type,
            Constraints = cargoObject.Constraints,
            Weight = cargoObject.Weight,
            Volume = cargoObject.Volume,
            Name = cargoObject.Name,
            Price = cargoObject.Price,
        };
    }

    public override async Task<CargoObject> GetCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
    {
        var cargo = await dbContext.Cargos.FindAsync(request.Id);
        if (cargo == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
        var cargoObject = CargoToCargoObject(cargo);
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
        return await Task.FromResult(listCargoObjects);
    }

    public override async Task<CargoObject> CreateCargo(CreateOrUpdateCargoRequest request, ServerCallContext context)
    {
        var cargoObject = request.Cargo;
        var cargo = CargoObjectToCargo(cargoObject);

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
        var cargoObject = CargoToCargoObject(cargoDB);

        return await Task.FromResult(cargoObject);
    }


    /*DBContext db;
    public UserApiService(DBContext db)
    {
        this.db = db;
    }

    // отправляем список пользователей
    public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
    {
        var listReply = new ListReply();    // определяем список
                                            // преобразуем каждый объект User в объект UserReply
        var userList = db.NewTestTables.Select(item => new UserReply { Id = item.Id, Name = item.StringData, Age = (int)item.IntData }).ToList();
        listReply.Users.AddRange(userList);
        return Task.FromResult(listReply);
    }
    // отправляем одного пользователя по id
    public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        // если пользователь не найден, генерируем исключение
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        UserReply userReply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(userReply);
    }

    // добавление пользователя
    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        // формируем из данных объект User и добавляем его в список users
        var user = new NewTestTable { StringData = request.Name, IntData = request.Age };
        await db.NewTestTables.AddAsync(user);
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(reply);
    }
    // обновление пользователя
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        // обновляем даннные
        user.StringData = request.Name;
        user.IntData = request.Age;
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(reply);
    }
    // удаление пользователя
    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        // удаляем пользователя из бд
        db.NewTestTables.Remove(user);
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(reply);
    }


    /*
     * =*=*=*=*=*=*=*=*=*=*=*=*=*
     * CRUD OPERATIONS FOR 
     * --- EMPLOYEES TABLE ---
     * =*=*=*=*=*=*=*=*=*=*=*=*=*
     */

    /*
    public override Task<EmployeeList> ListEmployee(Empty request, ServerCallContext context)
    {
        var listReply = new EmployeeList();

        var userList = db.Employees.Select(item => new EmployeeObject
        {
            Id = item.Id,
            Post = item.Post,
            Division = item.Division,
            Department = item.Department,
            Sector = item.Sector.GetValueOrDefault(-1),
            Name = item.Name,
            Patronymic = item.Patronymic,
            Surname = item.Surname,
            Role = item.Role,
            Skill = item.Skill
        }).ToList();

        listReply.Employees.AddRange(userList);
        return Task.FromResult(listReply);
    }

    public override async Task<EmployeeObject> GetEmployee(GetEmployeeRequest request, ServerCallContext context)
    {
        var item = await db.Employees.FindAsync(request.Id);
        if (item == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
        }
        EmployeeObject employeeObject = new EmployeeObject()
        {
            Id = item.Id,
            Post = item.Post,
            Division = item.Division,
            Department = item.Department,
            Sector = item.Sector.GetValueOrDefault(-1),
            Name = item.Name,
            Patronymic = item.Patronymic,
            Surname = item.Surname,
            Role = item.Role,
            Skill = item.Skill
        };
        return await Task.FromResult(employeeObject);
    }



    // добавление пользователя
    public override async Task<EmployeeObject> CreateEmployee(EmployeeObject request, ServerCallContext context)
    {
        var employee = new Employee()
        {
            //Id = request.Id,
            Post = request.Post,
            Division = request.Division,
            Department = request.Department,
            Sector = request.Sector,
            Name = request.Name,
            Patronymic = request.Patronymic,
            Surname = request.Surname,
            Role = request.Role,
            Skill = request.Skill
        };
        await db.Employees.AddAsync(employee);
        await db.SaveChangesAsync();
        var reply = new EmployeeObject()
        {
            Id = employee.Id,
            Post = employee.Post,
            Division = employee.Division,
            Department = employee.Department,
            Sector = employee.Sector.GetValueOrDefault(-1),
            Name = employee.Name,
            Patronymic = employee.Patronymic,
            Surname = employee.Surname,
            Role = employee.Role,
            Skill = employee.Skill
        };
        return await Task.FromResult(reply);
    }

    public override async Task<EmployeeObject> DeleteEmployee(DeleteEmployeeRequest request, ServerCallContext context)
    {
        var employee = await db.Employees.FindAsync(request.Id);
        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
        }
        // удаляем пользователя из бд
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        var reply = new EmployeeObject()
        {
            Id = employee.Id,
            Post = employee.Post,
            Division = employee.Division,
            Department = employee.Department,
            Sector = employee.Sector.GetValueOrDefault(-1),
            Name = employee.Name,
            Patronymic = employee.Patronymic,
            Surname = employee.Surname,
            Role = employee.Role,
            Skill = employee.Skill
        };
        return await Task.FromResult(reply);
    }

    // обновление пользователя
    public override async Task<EmployeeObject> UpdateEmployee(EmployeeObject request, ServerCallContext context)
    {
        var employee = await db.Employees.FindAsync(request.Id);
        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        // обновляем даннные
        employee.Post = request.Post;
        employee.Division = request.Division;
        employee.Department = request.Department;
        employee.Sector = request.Sector;
        employee.Name = request.Name;
        employee.Patronymic = request.Patronymic;
        employee.Surname = request.Surname;
        employee.Role = request.Role;
        employee.Skill = request.Skill;

        await db.SaveChangesAsync();
        var reply = new EmployeeObject()
        {
            Id = employee.Id,
            Post = employee.Post,
            Division = employee.Division,
            Department = employee.Department,
            Sector = employee.Sector.GetValueOrDefault(-1),
            Name = employee.Name,
            Patronymic = employee.Patronymic,
            Surname = employee.Surname,
            Role = employee.Role,
            Skill = employee.Skill
        };
        return await Task.FromResult(reply);
    }

    /*
    * =*=*=*=*=*=*=*=*=*=*=*=*=*
    * CRUD OPERATIONS FOR 
    * --- ROLES TABLE ---
    * =*=*=*=*=*=*=*=*=*=*=*=*=*
    */
    /*
    public override async Task<ListRoleRequest> GetListRole(Empty request, ServerCallContext context)
    {
        var listReply = new ListRoleRequest();

        var rolesList = db.Roles.Select(item => new RoleObject
        {
            Id = item.Id,
            Name = item.Name,
        }).ToList();

        listReply.RoleObject.AddRange(rolesList);
        return await Task.FromResult(listReply);
    }

    public override async Task<RoleObject> GetRole(GetRoleRequest request, ServerCallContext context)
    {
        var item = await db.Roles.FindAsync(request.Id);
        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Role not found"));
        RoleObject roleObject = new RoleObject()
        {
            Id = item.Id,
            Name = item.Name
        };
        return await Task.FromResult(roleObject);
    }


    //private readonly ILogger<GreeterService> _logger;
    //public GreeterService(ILogger<GreeterService> logger)
    //{
    //    _logger = logger;
    //}

    //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    //{
    //    return Task.FromResult(new HelloReply
    //    {
    //        Message = "Hello " + request.Name
    //    });
    //}
    */
}