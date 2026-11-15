# LogisticsApiServices

Серверная часть информационной системы «Моя логистика». Разработана в рамках дипломного проекта по специальности 09.02.07 «Информационные системы и программирование».

Сервер отвечает за хранение данных в СУБД PostgreSQL, валидацию запросов, аутентификацию пользователей и предоставление API для клиентского WPF-приложения.

## Основной функционал

* **gRPC API:** реализация удаленных процедур (RPC) для CRUD-операций со всеми сущностями системы (грузы, заявки, маршруты, контрагенты, транспорт, водители).
* **Безопасность:** авторизация на базе JWT-токенов, хеширование паролей по алгоритму SHA-512.
* **Доступ к данным:** работа с PostgreSQL через ORM Entity Framework Core.
* **Конвертеры:** преобразование объектов Protobuf в модели EF Core и обратно.

## Стек технологий

* **Платформа:** ASP.NET Core (.NET 7) / C#
* **Сетевой протокол:** gRPC (HTTP/2)
* **ORM:** Entity Framework Core
* **СУБД:** PostgreSQL (Npgsql)
* **Безопасность:** JWT (Bearer), SHA-512

## Связанные части проекта

* Клиентское приложение (WPF): [LogisticsClientsApp](https://github.com/Grigory-Mag/LogisticsClientsApp)
* Модульные тесты: [LogisticsUnitTests](https://github.com/Grigory-Mag/LogisticsUnitTests)

## Развертывание базы данных

В проекте используется СУБД **PostgreSQL**. Готовый SQL-скрипт создания схемы, таблиц и связей находится в папке [`Database/init_db.sql`](Database/init_db.sql).

### Порядок инициализации:
1. Создайте пустую базу данных в PostgreSQL (например, `logistics_db`).
2. Выполните скрипт `Database/init_db.sql` через **DataGrip**, **pgAdmin** или консоль:
   ```bash
   psql -U postgres -d logistics_db -f Database/init_db.sql
3. Укажите параметры подключения в файле appsettings.json:

`"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=logistics_db;Username=postgres;Password=ваш_пароль"
}`

## Запуск сервера

1. Развернуть базу данных PostgreSQL и накатить схему (скрипт создания таблиц находится в документации/проекте).
2. Настроить строку подключения к БД в `appsettings.json`.
3. Запустить проект через Visual Studio или консоль:
   ```bash
   dotnet run
