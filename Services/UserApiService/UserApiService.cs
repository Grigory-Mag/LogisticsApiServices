global using LogisticsApiServices.DBPostModels;
global using Task = System.Threading.Tasks.Task;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter;
using ApiService;

namespace ApiService;
public partial class UserApiService : UserService.UserServiceBase
{
    DBContext dbContext;
    public UserApiService(DBContext db)
    {
        this.dbContext = db;
    }
}