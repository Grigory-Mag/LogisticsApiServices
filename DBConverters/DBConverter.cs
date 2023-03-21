using ApiService;
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
                Constraints = cargo.Constraints,
                Weight = cargo.Weight,
                Volume = cargo.Volume,
                Name = cargo.Name,
                Price = cargo.Price,
            };
        }
    }

    public partial class CargoConstraint
    {
        public static explicit operator CargoConstraintsObject (CargoConstraint cargoConstraint)
        {
            return new CargoConstraintsObject()
            {
                IdCargo = cargoConstraint.IdCargo,
                IdConstraint = cargoConstraint.IdConstraint
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

    public partial class Constraint
    {
        public static explicit operator ConstraintsObject(Constraint constraint)
        {
            return new ConstraintsObject()
            {
                Id = constraint.Id,
                Desc = constraint.Desc
            };
        }
    }

    public partial class Customer
    {
        public static explicit operator CustomersObject(Customer customer)
        {
            return new CustomersObject()
            {
                Id = customer.Id,
                Cargo = customer.Cargo,
                Requisite = customer.Requisite
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
                Licence = driver.Licence,
                Patronymic = driver.Patronymic,
                Sanitation = driver.Sanitation,
                Surname = driver.Surname
            };
        }
    }

    public partial class DriverLicence
    {
        public static explicit operator DriverLicenceObject(DriverLicence driverLicence)
        {
            return new DriverLicenceObject()
            {
                Id=driverLicence.Id,
                Series = driverLicence.Series,
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(driverLicence.Date),
                Number = driverLicence.Number
            };
        }
    }

    public partial class Order
    {

    }

    public partial class Ownership
    {

    }

    public partial class Request
    {

    }

    public partial class Requisite
    {

    }

    public partial class Transporter
    {

    }

    public partial class Vehicle
    {

    }

    public partial class VehicleType
    {

    }
}
