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
    
    public override async Task<CargoObject> GetCargo(GetOrDeleteCargoRequest request, ServerCallContext context)
    {
        // ��������� �� ������ ������ User � ��������� ��� � ������ users
        var cargo = await dbContext.Cargos.FindAsync(request.Id);
        if (cargo == null)        
            throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));

        CargoObject cargoObject = new CargoObject()
        {
            Id = cargo.Id,
            Type = cargo.Type,
            Constraints = cargo.Constraints,
            Weight = cargo.Weight,
            Volume = cargo.Volume,
            Name = cargo.Name,
            Price = cargo.Price,
        };

        return await Task.FromResult(cargoObject);
        
        /* var user = new NewTestTable { StringData = request.Name, IntData = request.Age };
        await db.NewTestTables.AddAsync(user);
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(cargo); */
    }


    /*DBContext db;
    public UserApiService(DBContext db)
    {
        this.db = db;
    }

    // ���������� ������ �������������
    public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
    {
        var listReply = new ListReply();    // ���������� ������
                                            // ����������� ������ ������ User � ������ UserReply
        var userList = db.NewTestTables.Select(item => new UserReply { Id = item.Id, Name = item.StringData, Age = (int)item.IntData }).ToList();
        listReply.Users.AddRange(userList);
        return Task.FromResult(listReply);
    }
    // ���������� ������ ������������ �� id
    public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        // ���� ������������ �� ������, ���������� ����������
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        UserReply userReply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(userReply);
    }

    // ���������� ������������
    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        // ��������� �� ������ ������ User � ��������� ��� � ������ users
        var user = new NewTestTable { StringData = request.Name, IntData = request.Age };
        await db.NewTestTables.AddAsync(user);
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(reply);
    }
    // ���������� ������������
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        // ��������� �������
        user.StringData = request.Name;
        user.IntData = request.Age;
        await db.SaveChangesAsync();
        var reply = new UserReply() { Id = user.Id, Name = user.StringData, Age = (int)user.IntData };
        return await Task.FromResult(reply);
    }
    // �������� ������������
    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await db.NewTestTables.FindAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }
        // ������� ������������ �� ��
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



        // ���������� ������������
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
            // ������� ������������ �� ��
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

        // ���������� ������������
        public override async Task<EmployeeObject> UpdateEmployee(EmployeeObject request, ServerCallContext context)
        {
            var employee = await db.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            // ��������� �������
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