using LogisticsApiServices.DBPostModels;

namespace ApiService
{
 public sealed partial class CargoObject
    {
        public static explicit operator Cargo(CargoObject cargoObject)
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
    }
 public sealed partial class ListCargo
    {

    }
 public sealed partial class CargoConstraintsObject
    {
        public static explicit operator CargoConstraint(CargoConstraintsObject cargoConstraintObject)
        {
            return new CargoConstraint()
            {
                IdCargo = cargoConstraintObject.IdCargo,
                IdConstraint = cargoConstraintObject.IdConstraint
            };
        }
    } 

 public sealed partial class CargoTypesObject
    {
        public static explicit operator CargoType(CargoTypesObject cargoTypeObject)
        {
            return new CargoType()
            {
                Id = cargoTypeObject.Id,
                Name = cargoTypeObject.Name
            };
        }
    }

 public sealed partial class ConstraintsObject
    {
        public static explicit operator Constraint(ConstraintsObject constraint)
        {
            return new Constraint()
            {
                Id = constraint.Id,
                Desc = constraint.Desc
            };
        }
    }

 public sealed partial class ListConstraints
    {

    }

 public sealed partial class CustomersObject
    {
        public static explicit operator Customer(CustomersObject customer)
        {
            return new Customer()
            {
                Id = customer.Id,
                Cargo = customer.Cargo,
                Requisite = customer.Requisite
            };
        }
    }

 public sealed partial class ListCustomers
    {

    }

 public sealed partial class DriverLicenceObject
    {
        public static explicit operator DriverLicence(DriverLicenceObject driverLicence)
        {
            return new DriverLicence()
            {
                Id = driverLicence.Id,
                Series = driverLicence.Series,
                Date = driverLicence.Date.ToDateTime(),
                Number = driverLicence.Number,
            };
        }
    }

 public sealed partial class ListDriverLicence
    {

    }

 public sealed partial class DriversObject
    {
        public static explicit operator Driver(DriversObject driver)
        {
            return new Driver()
            {
                Id = driver.Id,
                Name = driver.Name,
                Licence = driver.Licence,
                Patronymic = driver.Patronymic,
                Sanitation = driver.Sanitation,
                Surname = driver.Surname
            };
        }
    }

 public sealed partial class ListDrivers
    {

    }
 
 public sealed partial class OrdersObject
    {
        public static explicit operator Order(OrdersObject order)
        {
            return new Order()
            {
                Id = order.Id,
                Cargo = order.Cargo,
                Date = order.Date.ToDateTime(),
            };
        }
    }
 
 public sealed partial class ListOrders
    {

    }
 
 public sealed partial class OwnershipsObject
    {
        public static explicit operator Ownership(OwnershipsObject ownership)
        {
            return new Ownership()
            {
                Id = ownership.Id,
                Name = ownership.Name,
            };
        }
    }
 
 public sealed partial class ListOwnerships
    {

    }
 
 public sealed partial class RequestsObject
    {
        public static explicit operator Request(RequestsObject request)
        {
            return new Request()
            {
                Id = request.Id,
                Conditions = request.Conditions,
                Order = request.Order,
                Price = request.Price,
                Vehicle = request.Vehicle,
            };
        }
    }
 
 public sealed partial class ListRequest
    {

    }
 
 public sealed partial class RequisitesObject
    {
        public static explicit operator Requisite(RequisitesObject requisite)
        {
            return new Requisite()
            {
                Id = requisite.Id,
                Inn = requisite.Inn,
                Ceo = requisite.Ceo,
                LegalAddress = requisite.LegalAddress,
                Ownership = requisite.Ownership,
                Pts = requisite.Pts,
            };
        }
    }
 
 public sealed partial class ListRequisites
    {

    }
 
 public sealed partial class TransportersObject
    {
        public static explicit operator Transporter(TransportersObject requisite)
        {
            return new Transporter()
            {
                Id = requisite.Id,
                Name = requisite.Name
            };
        }
    }
 
 public sealed partial class ListTransporters
    {

    }
 
 public sealed partial class TransportersVehiclesObject
    {

    }
 
 public sealed partial class ListTransportersVehicles
    {

    }
 
 public sealed partial class VehiclesTypesObject
    {

    }
 
 public sealed partial class ListVehiclesTypes
    {

    }
 
 public sealed partial class VehiclesObject
    {
        public static explicit operator Vehicle(VehiclesObject requisite)
        {
            return new Vehicle()
            {
                Id = requisite.Id,
                Driver = requisite.Driver,
                Number = requisite.Number,
                Owner = requisite.Owner,
                Type = requisite.Type,
            };
        }
    }
 
 public sealed partial class ListVehicles
    {

    }
 
 public sealed partial class VehiclesTransportersObject
    {

    }
 
 public sealed partial class ListVehiclesTransporters
    {

    }
 
}
