using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApiService
{
    public partial class UserApiService
    {
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

            return await Task.FromResult((CustomersObject)customer);
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
    }
}
