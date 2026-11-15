CREATE SCHEMA IF NOT EXISTS logistics_shema;
SET search_path TO logistics_shema, public;




create sequence "Requisites_Id_seq"; alter sequence "Requisites_Id_seq" owner to postgres; create sequence "Drivers_Id_seq"; alter sequence "Drivers_Id_seq" owner to postgres; create sequence "Cargo_types_id_seq"; alter sequence "Cargo_types_id_seq" owner to postgres; create sequence "Cargo_id_seq"; alter sequence "Cargo_id_seq" owner to postgres; create sequence "Driver_Licence_id_seq"; alter sequence "Driver_Licence_id_seq" owner to postgres; create sequence "Vehicles_id_seq"; alter sequence "Vehicles_id_seq" owner to postgres; create sequence "Vehicle_Types_id_seq"; alter sequence "Vehicle_Types_id_seq" owner to postgres; create sequence "Organiztion_Roles_id_seq"     as integer;

alter sequence "Organiztion_Roles_id_seq" owner to postgres;

create table if not exists "Cargo_Types"
(
    id   integer default nextval('logistics_shema."Cargo_types_id_seq"'::regclass) not null
        constraint "Cargo_types_pk"
            primary key,
    name text                                                                      not null
);

alter table "Cargo_Types"
    owner to postgres;

alter sequence "Cargo_types_id_seq" owned by "Cargo_Types".id;

create table if not exists "Cargo"
(
    id          integer default nextval('logistics_shema."Cargo_id_seq"'::regclass) not null
        constraint "Cargo_pk"
            primary key,
    type        integer
        constraint "Cargo_Cargo_types_id_fk"
            references "Cargo_Types",
    weight      double precision,
    volume      double precision,
    name        text,
    price       double precision,
    constraints text
);

alter table "Cargo"
    owner to postgres;

alter sequence "Cargo_id_seq" owned by "Cargo".id;

create table if not exists "Driver_Licence"
(
    id     integer default nextval('logistics_shema."Driver_Licence_id_seq"'::regclass) not null
        constraint "Driver_Licence_pk"
            primary key,
    series integer,
    number integer,
    date   timestamp                                                                    not null
);

alter table "Driver_Licence"
    owner to postgres;

alter sequence "Driver_Licence_id_seq" owned by "Driver_Licence".id;

create table if not exists "Drivers"
(
    id         integer default nextval('logistics_shema."Drivers_Id_seq"'::regclass) not null
        constraint "Drivers_pk"
            primary key,
    name       text                                                                  not null,
    surname    text                                                                  not null,
    patronymic text,
    sanitation boolean                                                               not null,
    licence    integer                                                               not null
        constraint "Drivers_Driver_Licence_id_fk"
            references "Driver_Licence"
);

alter table "Drivers"
    owner to postgres;

alter sequence "Drivers_Id_seq" owned by "Drivers".id;

create table if not exists "Vehicle_Types"
(
    id   integer default nextval('logistics_shema."Vehicle_Types_id_seq"'::regclass) not null
        constraint "Vehicle_Types_pk"
            primary key,
    name text
);

alter table "Vehicle_Types"
    owner to postgres;

alter sequence "Vehicle_Types_id_seq" owned by "Vehicle_Types".id;

create table if not exists "Route_Actions"
(
    action text not null,
    id     serial
        constraint "Route_Actions_pk"
            primary key
);

alter table "Route_Actions"
    owner to postgres;

create table if not exists "Route"
(
    id          serial
        constraint "Route_pk"
            primary key,
    address     text      not null,
    action      integer   not null
        constraint "Route_Route_Actions_id_fk"
            references "Route_Actions",
    action_date timestamp not null
);

alter table "Route"
    owner to postgres;

create table if not exists "Roles"
(
    id   integer default nextval('logistics_shema."Organiztion_Roles_id_seq"'::regclass) not null
        constraint "Organiztion_Roles_pk"
            primary key,
    name text
);

alter table "Roles"
    owner to postgres;

alter sequence "Organiztion_Roles_id_seq" owned by "Roles".id;

create table if not exists "Requisites_Types"
(
    id   serial
        constraint "Requisites_Types_pk"
            primary key,
    name text
);

alter table "Requisites_Types"
    owner to postgres;

create table if not exists "Requisites"
(
    id            integer default nextval('logistics_shema."Requisites_Id_seq"'::regclass) not null
        constraint "Requisites_pk"
            primary key,
    legal_address text                                                                     not null,
    "Inn"         varchar(12)                                                              not null,
    ceo           text                                                                     not null,
    pts           integer                                                                  not null,
    role          integer                                                                  not null
        constraint "Requisites_Roles_id_fk"
            references "Roles",
    name          text                                                                     not null,
    type          integer
        constraint "Requisites_Requisites_Types_id_fk"
            references "Requisites_Types"
);

alter table "Requisites"
    owner to postgres;

alter sequence "Requisites_Id_seq" owned by "Requisites".id;

create table if not exists "Vehicles"
(
    id             integer default nextval('logistics_shema."Vehicles_id_seq"'::regclass) not null
        constraint "Vehicles_pk"
            primary key,
    type           integer                                                                not null
        constraint "Vehicles_Vehicle_Types_id_fk"
            references "Vehicle_Types",
    number         text                                                                   not null,
    owner          integer                                                                not null
        constraint "Vehicles_Requisites_id_fk"
            references "Requisites",
    trailer_number text
);

alter table "Vehicles"
    owner to postgres;

alter sequence "Vehicles_id_seq" owned by "Vehicles".id;

create table if not exists "Requests"
(
    id                 serial
        constraint "Requests_pk"
            primary key,
    vehicle            integer
        constraint "Requests_Vehicles_id_fk"
            references "Vehicles",
    price              double precision,
    creation_date      timestamp not null,
    documents_original boolean,
    customer           integer
        constraint "Requests_Requisites_id_fk"
            references "Requisites",
    transporter        integer
        constraint "Requests_Requisites_id_fk2"
            references "Requisites",
    cargo              integer
        constraint "Requests_Cargo_id_fk"
            references "Cargo",
    is_finishied       boolean,
    driver             integer
        constraint "Requests_Drivers_id_fk"
            references "Drivers",
    constraint check_customer_transporter
        check (customer <> transporter)
);

alter table "Requests"
    owner to postgres;

create table if not exists "Routes_Requests"
(
    id_requests integer not null
        constraint "Routes_Requests_Requests_id_fk"
            references "Requests"
            on delete cascade,
    id_route    integer not null
        constraint "Routes_Requests_Route_id_fk"
            references "Route"
            on delete cascade,
    constraint "Routes_Requests_pk"
        primary key (id_requests, id_route)
);

alter table "Routes_Requests"
    owner to postgres;

create table if not exists "User_Roles"
(
    id   serial
        constraint "User_Roles_pk"
            primary key,
    name text
);

alter table "User_Roles"
    owner to postgres;

create table if not exists "Users"
(
    id         serial
        constraint "Users_pk"
            primary key,
    login      text
        constraint "Users_pk2"
            unique,
    password   text,
    name       text,
    surname    text,
    patronymic text,
    role       integer
        constraint "Users_User_Roles_id_fk"
            references "User_Roles"
);

alter table "Users"
    owner to postgres;
