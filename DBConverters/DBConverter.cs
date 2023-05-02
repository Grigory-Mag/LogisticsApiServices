using ApiService;
using Azure.Core;
using Google.Protobuf;
using System;
using System.Collections.Generic;


namespace LogisticsApiServices.DBPostModels
{
    public partial class Cargo
    {
        public static explicit operator CargoObject(Cargo cargo)
        {
            return new CargoObject()
            {
                Id = cargo.Id,
                Type = cargo.Type,
                Weight = cargo.Weight,
                Volume = cargo.Volume,
                Name = cargo.Name,
                Price = cargo.Price,
                Constraints = cargo.Constraints == null ? string.Empty : cargo.Constraints,
                CargoType = cargo.TypeNavigation == null ? null : (CargoTypesObject)cargo.TypeNavigation
            };
        }
    }

    public partial class CargoType
    {
        public static explicit operator CargoTypesObject(CargoType cargoType)
        {
            return new CargoTypesObject()
            {
                Id = cargoType.Id,
                Name = cargoType.Name
            };
        }
    }

    public partial class Driver
    {
        public static explicit operator DriversObject(Driver driver)
        {
            return new DriversObject()
            {
                Id = driver.Id,
                Name = driver.Name,
                Licence = driver.LicenceNavigation == null ? null : (DriverLicenceObject)driver.LicenceNavigation,
                Patronymic = driver.Patronymic == null ? string.Empty : driver.Patronymic,
                Sanitation = driver.Sanitation,
                Surname = driver.Surname == null ? string.Empty : driver.Surname
            };
        }
    }

    public partial class DriverLicence
    {
        public static explicit operator DriverLicenceObject(DriverLicence driverLicence)
        {
            return new DriverLicenceObject()
            {
                Id = driverLicence.Id,
                Series = driverLicence.Series,
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(driverLicence.Date.ToUniversalTime()),
                Number = driverLicence.Number
            };
        }
    }

    public partial class Request
    {
        public static explicit operator RequestsObject(Request request)
        {
            var routes = new ListRouteObjects();
            var routesList = request.IdRoutes.ToList();
            routesList.ForEach(item => routes.RouteObjects.Add((RouteObject)item));
            return new RequestsObject()
            {
                Id = request.Id,
                Price = request.Price,
                Driver = request.Driver == null ? null : (DriversObject)request.DriverNavigation,
                Vehicle = (VehiclesObject)request.VehicleNavigation,
                IsFinished = request.IsFinishied == null ? false : (bool)request.IsFinishied,
                CreationDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(request.CreationDate.ToUniversalTime()),
                Documents = request.DocumentsOriginal == null ? false : (bool)request.DocumentsOriginal,
                Cargo = request.CargoNavigation == null ? null : (CargoObject)request.CargoNavigation,
                CustomerReq = request.CustomerNavigation == null ? null : (RequisitesObject)request.CustomerNavigation,
                TransporterReq = request.TransporterNavigation == null ? null : (RequisitesObject)request.TransporterNavigation,
                Routes = routes,
            };
        }
    }

    public partial class Requisite
    {
        public static explicit operator RequisitesObject(Requisite requisite)
        {
            return new RequisitesObject()
            {
                Id = requisite.Id,
                Name = requisite.Name,
                Inn = requisite.Inn,
                Ceo = requisite.Ceo,
                LegalAddress = requisite.LegalAddress,
                Role = requisite.RoleNavigation == null ? null : (RolesObject)requisite.RoleNavigation,
                Pts = requisite.Pts,
            };
        }
    }

    public partial class Vehicle
    {
        public static explicit operator VehiclesObject(Vehicle item)
        {
            return new VehiclesObject()
            {
                Id = item.Id,
                Number = item.Number,
                Owner = item.OwnerNavigation == null ? null : (RequisitesObject)item.OwnerNavigation,
                Type = item.OwnerNavigation == null ? null : (VehiclesTypesObject)item.TypeNavigation,
                TrailerNumber = item.TrailerNumber,
            };
        }
    }

    public partial class VehicleType
    {
        public static explicit operator VehiclesTypesObject(VehicleType item)
        {
            return new VehiclesTypesObject()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }

    public partial class Role
    {
        public static explicit operator RolesObject(Role role)
        {
            return new RolesObject()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }

    public partial class Route
    {
        public static explicit operator RouteObject(Route route)
        {
            return new RouteObject()
            {
                Id = route.Id,
                Address = route.Address,
                ActionDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(route.ActionDate.ToUniversalTime()),
                Action = (RouteActionsObject)route.ActionNavigation,
            };
        }
    }

    public partial class RouteAction
    {
        public static explicit operator RouteActionsObject(RouteAction route)
        {
            return new RouteActionsObject()
            {
                Id = route.Id,
                Action = route.Action,
            };
        }
    }

}