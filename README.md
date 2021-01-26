# BooksStore2021

Live on [Azure](https://booksstore2021.azurewebsites.net/)

## History

2018.08 Started a .Net FrameWork MVC project [BooksStoreDotnetMVC](https://github.com/daviddongguo/BooksStoreDotnetMVC)

2020.12 Refactored targeting on .Net5

## New technologies applied

- .Net Core 5 Project

- Use [scripts](https://github.com/daviddongguo/BooksStore2021/tree/main/scripts) to create related empty projects automatically.  See also [dotnetcore.template](https://github.com/daviddongguo/dotnetcore.template)

- Apply [git Actions](https://github.com/daviddongguo/BooksStore2021/tree/main/.github/workflows) for testing and deploying

- Connection to MySql by using  MySql.Data.EntityFrameworkCore v8.0.22, MySqlConnector v0.69.1, and Microsoft.AspNetCore.Identity.EntityFrameworkCore v3.1.10

- Repository Pattern and UnitOfWork on [Domain](https://github.com/daviddongguo/BooksStore2021/tree/main/src/BooksStore2021.Domain)

- [Sessions](https://github.com/daviddongguo/BooksStore2021/blob/main/src/BooksStore2021.Utility/SessionExtensions.cs) in .NET Core

- Data Seeding on [Domain/Initializer](https://github.com/daviddongguo/BooksStore2021/tree/main/src/BooksStore2021.Domain/Initializer), [Mvc/Startup](https://github.com/daviddongguo/BooksStore2021/blob/main/src/BooksStore2021.Mvc/Startup.cs)

- IdentityUser on [Mvc/Areas/Identity/Pages/Account/Register](https://github.com/daviddongguo/BooksStore2021/blob/main/src/BooksStore2021.Mvc/Areas/Identity/Pages/Account/Register.cshtml.cs)

- Facebook Single Sign On
