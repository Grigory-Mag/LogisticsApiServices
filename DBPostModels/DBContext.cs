using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LogisticsApiServices.DBPostModels
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<CargoConstraint> CargoConstraints { get; set; } = null!;
        public virtual DbSet<CargoType> CargoTypes { get; set; } = null!;
        public virtual DbSet<Constraint> Constraints { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Driver> Drivers { get; set; } = null!;
        public virtual DbSet<DriverLicence> DriverLicences { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Ownership> Ownerships { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Requisite> Requisites { get; set; } = null!;
        public virtual DbSet<Transporter> Transporters { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
        public virtual DbSet<VehicleType> VehicleTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=logistics;Username=postgres;Password=12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("Cargo", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Constraints).HasColumnName("constraints");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("Cargo_Cargo_types_id_fk");
            });

            modelBuilder.Entity<CargoConstraint>(entity =>
            {
                entity.HasKey(e => new { e.IdConstraint, e.IdCargo })
                    .HasName("Cargo_Constraints_pk");

                entity.ToTable("Cargo_Constraints", "logistics_shema");

                entity.HasIndex(e => e.IdCargo, "Cargo_Constraints_pk2")
                    .IsUnique();

                entity.Property(e => e.IdConstraint).HasColumnName("id_constraint");

                entity.Property(e => e.IdCargo).HasColumnName("id_cargo");

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithOne(p => p.CargoConstraint)
                    .HasForeignKey<CargoConstraint>(d => d.IdCargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cargo_Constraints_Cargo_id_fk");

                entity.HasOne(d => d.IdConstraintNavigation)
                    .WithMany(p => p.CargoConstraints)
                    .HasForeignKey(d => d.IdConstraint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cargo_Constraints_Constraints_id_fk");
            });

            modelBuilder.Entity<CargoType>(entity =>
            {
                entity.ToTable("Cargo_Types", "logistics_shema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('logistics_shema.\"Cargo_types_id_seq\"'::regclass)");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Constraint>(entity =>
            {
                entity.ToTable("Constraints", "logistics_shema");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Desc).HasColumnName("desc");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cargo).HasColumnName("cargo");

                entity.Property(e => e.Requisite).HasColumnName("requisite");

                entity.HasOne(d => d.CargoNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Cargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Customers___fk");

                entity.HasOne(d => d.RequisiteNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Requisite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Customers_Requisites_Id_fk");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Drivers", "logistics_shema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('logistics_shema.\"Drivers_Id_seq\"'::regclass)");

                entity.Property(e => e.Licence).HasColumnName("licence");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Patronymic).HasColumnName("patronymic");

                entity.Property(e => e.Sanitation).HasColumnName("sanitation");

                entity.Property(e => e.Surname).HasColumnName("surname");

                entity.HasOne(d => d.LicenceNavigation)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.Licence)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Drivers_Driver_Licence_id_fk");
            });

            modelBuilder.Entity<DriverLicence>(entity =>
            {
                entity.ToTable("Driver_Licence", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Series).HasColumnName("series");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cargo).HasColumnName("cargo");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.HasOne(d => d.CargoNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Cargo)
                    .HasConstraintName("Orders_Cargo_id_fk");
            });

            modelBuilder.Entity<Ownership>(entity =>
            {
                entity.ToTable("Ownerships", "logistics_shema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('logistics_shema.\"Ownerships_Id_seq\"'::regclass)");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Requests", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Conditions).HasColumnName("conditions");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Vehicle).HasColumnName("vehicle");

                entity.HasOne(d => d.OrderNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.Order)
                    .HasConstraintName("Requests_Orders_id_fk");

                entity.HasOne(d => d.VehicleNavigation)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.Vehicle)
                    .HasConstraintName("Requests_Vehicles_id_fk");
            });

            modelBuilder.Entity<Requisite>(entity =>
            {
                entity.ToTable("Requisites", "logistics_shema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('logistics_shema.\"Requisites_Id_seq\"'::regclass)");

                entity.Property(e => e.Ceo).HasColumnName("ceo");

                entity.Property(e => e.Inn).HasMaxLength(12);

                entity.Property(e => e.LegalAddress).HasColumnName("legal_address");

                entity.Property(e => e.Ownership).HasColumnName("ownership");

                entity.Property(e => e.Pts).HasColumnName("pts");

                entity.HasOne(d => d.OwnershipNavigation)
                    .WithMany(p => p.Requisites)
                    .HasForeignKey(d => d.Ownership)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Requisites_Ownerships_Id_fk");
            });

            modelBuilder.Entity<Transporter>(entity =>
            {
                entity.ToTable("Transporters", "logistics_shema");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('logistics_shema.transporters_id_seq'::regclass)");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.HasMany(d => d.VehiclesNavigation)
                    .WithMany(p => p.TransportersNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "VehiclesTransporter",
                        l => l.HasOne<Vehicle>().WithMany().HasForeignKey("Vehicle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Vehicles_Transporters_Vehicles_id_fk"),
                        r => r.HasOne<Transporter>().WithMany().HasForeignKey("Transporter").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Vehicles_Transporters_Transporters_id_fk"),
                        j =>
                        {
                            j.HasKey("Transporter", "Vehicle").HasName("Vehicles_Transporters_pk");

                            j.ToTable("Vehicles_Transporters", "logistics_shema");

                            j.IndexerProperty<int>("Transporter").HasColumnName("transporter");

                            j.IndexerProperty<int>("Vehicle").HasColumnName("vehicle");
                        });
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Driver).HasColumnName("driver");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.HasOne(d => d.DriverNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.Driver)
                    .HasConstraintName("Vehicles_Drivers_id_fk");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("Vehicles_Ownerships_id_fk");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("Vehicles_Vehicle_Types_id_fk");

                entity.HasMany(d => d.Transporters)
                    .WithMany(p => p.Vehicles)
                    .UsingEntity<Dictionary<string, object>>(
                        "TransportersVehicle",
                        l => l.HasOne<Transporter>().WithMany().HasForeignKey("Transporter").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Transporters_Vehicles_Transporters_id_fk"),
                        r => r.HasOne<Vehicle>().WithMany().HasForeignKey("Vehicle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Transporters_Vehicles_Vehicles_id_fk"),
                        j =>
                        {
                            j.HasKey("Vehicle", "Transporter").HasName("Transporters_Vehicles_pk");

                            j.ToTable("Transporters_Vehicles", "logistics_shema");

                            j.IndexerProperty<int>("Vehicle").HasColumnName("vehicle");

                            j.IndexerProperty<int>("Transporter").HasColumnName("transporter");
                        });
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.ToTable("Vehicle_Types", "logistics_shema");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
