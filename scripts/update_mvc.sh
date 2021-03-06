#!/usr/bin/env bash
Root=${PWD##*/}

cd $Root.MVC && pwd
dotnet add package Microsoft.EntityFrameworkCore.Tools -v 3.1.10
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 3.1.10

dotnet add package MySql.Data.EntityFrameworkCore -v 8.0.22
dotnet add package MySqlConnector -v 0.69.10

cd ./Controllers
cat > TodoesController.cs << EOF
using $Root.Domain.Entities;
using $Root.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace $Root.Mvc.Controllers
{
    public class TodoesController : Controller
    {
        private EFDbContext _db;

        public TodoesController()
        {
            var options = new DbContextOptionsBuilder<EFDbContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;

            _db = new EFDbContext(options);
            _db.Todoes.Add(new Todo
            {
                Id = "18kxytxr0cak4ay1nj4d",
                Title = "18kxytxr0cak4ay1nj4d",
            });
            _db.Add(new Todo
            {
                Id = "228kxytxr0cak4ay1nj4d",
                Title = "228kxytxr0cak4ay1nj4d",
            });
            _db.SaveChanges();
        }

        // GET: TodoesController1
        public ActionResult Index()
        {
            var Todoes = _db.Todoes;
            return View(Todoes);
        }

    }
}
EOF
cd ..
mkdir Views/Todoes

cd ./Views/Todoes/
cat > Index.cshtml << EOF
@model IEnumerable<$Root.Domain.Entities.Todo>
<div>
  @foreach (var item in Model)
  {
    <div>@item.ToString()</div>

  }
</div>
EOF
cd ../../..
