using LogisticsApiServices.DBPostModels;
using System.Reflection.Metadata.Ecma335;

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
                Weight = cargoObject.Weight,
                Volume = cargoObject.Volume,
                Name = cargoObject.Name,
                Price = cargoObject.Price,
                Constraints = cargoObject.Constraints == string.Empty ? null : cargoObject.Constraints,
                TypeNavigation = cargoObject.CargoType == null ? null : (CargoType)cargoObject.CargoType,
            };
        }
    }
    public sealed partial class ListCargo
    {

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

    public sealed partial class ListConstraints
    {

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
                LicenceNavigation = driver.Licence == null ? null : (DriverLicence)driver.Licence,
                Patronymic = driver.Patronymic,
                Sanitation = driver.Sanitation,
                Surname = driver.Surname
            };
        }
    }

    public sealed partial class ListDrivers
    {

    }

    public sealed partial class ListOrders
    {

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
                Price = request.Price,
                DriverNavigation = request.Driver == null ? null : (Driver)request.Driver,
                VehicleNavigation = request.Vehicle == null ? null : (Vehicle)request.Vehicle,
                CargoNavigation = request.Cargo == null ? null : (Cargo)request.Cargo,
                CustomerNavigation = request.CustomerReq == null ? null : (Requisite)request.CustomerReq,
                TransporterNavigation = request.TransporterReq == null ? null : (Requisite)request.TransporterReq,
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
                Name = requisite.Name,
                Inn = requisite.Inn,
                Ceo = requisite.Ceo,
                LegalAddress = requisite.LegalAddress,
                RoleNavigation = requisite.Role == null ? null : (Role)requisite.Role,
                Pts = requisite.Pts == null ? 0 : (int)requisite.Pts,
                TypeNavigation = requisite.Type == null ? null : (RequisitesType)requisite.Type,
            };
        }
    }

    public sealed partial class ListRequisites
    {

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

    public sealed partial class ListVehiclesTypes
    {

    }

    public sealed partial class VehiclesObject
    {
        public static explicit operator Vehicle(VehiclesObject vehicle)
        {
            return new Vehicle()
            {
                Id = vehicle.Id,
                Number = vehicle.Number,
                Owner = vehicle.Owner.Id,
                Type = vehicle.Type.Id,
                TrailerNumber = vehicle.TrailerNumber,
            };
        }
    }

    public sealed partial class VehiclesTypesObject
    {
        public static explicit operator VehicleType(VehiclesTypesObject item)
        {
            return new VehicleType()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }

    public sealed partial class ListVehicles
    {

    }

    public sealed partial class ListVehiclesTransporters
    {

    }

    public partial class RolesObject
    {
        public static explicit operator Role(RolesObject role)
        {
            return new Role()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }

    public partial class RouteObject
    {
        public static explicit operator LogisticsApiServices.DBPostModels.Route(RouteObject route)
        {
            return new LogisticsApiServices.DBPostModels.Route()
            {
                Id = route.Id,
                Address = route.Address,
                ActionDate = route.ActionDate.ToDateTime(),
                ActionNavigation = route.Action == null ? null : (RouteAction)route.Action,
            };
        }
    }

    public partial class RouteActionsObject
    {
        public static explicit operator RouteAction(RouteActionsObject route)
        {
            return new RouteAction()
            {
                Id = route.Id,
                Action = route.Action,
            };
        }
    }

    public partial class RequisiteTypeObject
    {
        public static explicit operator RequisitesType(RequisiteTypeObject item)
        {
            return new RequisitesType()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }

    public partial class UserRoleObject
    {
        public static explicit operator UserRole (UserRoleObject item)
        {
            return new UserRole()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }

    public partial class LoginObject
    {
        public static explicit operator User (LoginObject item)
        {
            return new User()
            {
                Login = item.Login,
                Name = item.Name,
                Password = item.Password,
                Patronymic = item.Patronymic,
                Surname = item.Surname,
                RoleNavigation = item.UserRole == null ? null : (UserRole)item.UserRole,
            };
        }
    }
}
