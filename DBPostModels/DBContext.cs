using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApiServices.DBPostModels;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<CargoType> CargoTypes { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<DriverLicence> DriverLicences { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requisite> Requisites { get; set; }

    public virtual DbSet<RequisitesType> RequisitesTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteAction> RouteActions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = configurationBuilder.Build();
            var connection = _configuration.GetConnectionString("Postgres");

            optionsBuilder.UseNpgsql(connection);

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cargo_pk");

            entity.ToTable("Cargo", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Constraints).HasColumnName("constraints");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Volume).HasColumnName("volume");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("Cargo_Cargo_types_id_fk");
        });

        modelBuilder.Entity<CargoType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cargo_types_pk");

            entity.ToTable("Cargo_Types", "logistics_shema");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('logistics_shema.\"Cargo_types_id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Drivers_pk");

            entity.ToTable("Drivers", "logistics_shema");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('logistics_shema.\"Drivers_Id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Licence).HasColumnName("licence");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Sanitation).HasColumnName("sanitation");
            entity.Property(e => e.Surname).HasColumnName("surname");

            entity.HasOne(d => d.LicenceNavigation).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.Licence)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Drivers_Driver_Licence_id_fk");
        });

        modelBuilder.Entity<DriverLicence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Driver_Licence_pk");

            entity.ToTable("Driver_Licence", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Series).HasColumnName("series");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Requests_pk");

            entity.ToTable("Requests", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cargo).HasColumnName("cargo");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.Customer).HasColumnName("customer");
            entity.Property(e => e.DocumentsOriginal).HasColumnName("documents_original");
            entity.Property(e => e.Driver).HasColumnName("driver");
            entity.Property(e => e.IsFinishied).HasColumnName("is_finishied");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Transporter).HasColumnName("transporter");
            entity.Property(e => e.Vehicle).HasColumnName("vehicle");

            entity.HasOne(d => d.CargoNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Cargo)
                .HasConstraintName("Requests_Cargo_id_fk");

            entity.HasOne(d => d.CustomerNavigation).WithMany(p => p.RequestCustomerNavigations)
                .HasForeignKey(d => d.Customer)
                .HasConstraintName("Requests_Requisites_id_fk");

            entity.HasOne(d => d.DriverNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Driver)
                .HasConstraintName("Requests_Drivers_id_fk");

            entity.HasOne(d => d.TransporterNavigation).WithMany(p => p.RequestTransporterNavigations)
                .HasForeignKey(d => d.Transporter)
                .HasConstraintName("Requests_Requisites_id_fk2");

            entity.HasOne(d => d.VehicleNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Vehicle)
                .HasConstraintName("Requests_Vehicles_id_fk");

            entity.HasMany(d => d.IdRoutes).WithMany(p => p.IdRequests)
                .UsingEntity<Dictionary<string, object>>(
                    "RoutesRequest",
                    r => r.HasOne<Route>().WithMany()
                        .HasForeignKey("IdRoute")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Routes_Requests_Route_id_fk"),
                    l => l.HasOne<Request>().WithMany()
                        .HasForeignKey("IdRequests")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Routes_Requests_Requests_id_fk"),
                    j =>
                    {
                        j.HasKey("IdRequests", "IdRoute").HasName("Routes_Requests_pk");
                        j.ToTable("Routes_Requests", "logistics_shema");
                        j.IndexerProperty<int>("IdRequests").HasColumnName("id_requests");
                        j.IndexerProperty<int>("IdRoute").HasColumnName("id_route");
                    });
        });

        modelBuilder.Entity<Requisite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Requisites_pk");

            entity.ToTable("Requisites", "logistics_shema");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('logistics_shema.\"Requisites_Id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Ceo).HasColumnName("ceo");
            entity.Property(e => e.Inn).HasMaxLength(12);
            entity.Property(e => e.LegalAddress).HasColumnName("legal_address");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Pts).HasColumnName("pts");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Requisites)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Requisites_Roles_id_fk");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Requisites)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("Requisites_Requisites_Types_id_fk");
        });

        modelBuilder.Entity<RequisitesType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Requisites_Types_pk");

            entity.ToTable("Requisites_Types", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Organiztion_Roles_pk");

            entity.ToTable("Roles", "logistics_shema");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('logistics_shema.\"Organiztion_Roles_id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Route_pk");

            entity.ToTable("Route", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.ActionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("action_date");
            entity.Property(e => e.Address).HasColumnName("address");

            entity.HasOne(d => d.ActionNavigation).WithMany(p => p.Routes)
                .HasForeignKey(d => d.Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Route_Route_Actions_id_fk");
        });

        modelBuilder.Entity<RouteAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Route_Actions_pk");

            entity.ToTable("Route_Actions", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pk");

            entity.ToTable("Users", "logistics_shema");

            entity.HasIndex(e => e.Login, "Users_pk2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Surname).HasColumnName("surname");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("Users_User_Roles_id_fk");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_Roles_pk");

            entity.ToTable("User_Roles", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vehicles_pk");

            entity.ToTable("Vehicles", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Owner).HasColumnName("owner");
            entity.Property(e => e.TrailerNumber).HasColumnName("trailer_number");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.Owner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_Requisites_id_fk");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicles_Vehicle_Types_id_fk");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vehicle_Types_pk");

            entity.ToTable("Vehicle_Types", "logistics_shema");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
