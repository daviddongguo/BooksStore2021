#!/usr/bin/env bash
Root=${PWD##*/}

cd $Root.MVC && pwd
dotnet add package Microsoft.EntityFrameworkCore.Tools -v 3.1.10
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 3.1.10

cd ./Controllers
cat > TodoesController.cs << EOF
using $Root.Classlib.Entities;
using $Root.Classlib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace $Root.Mvc.Controllers
{
    public class TodoesController : Controller
    {
        private EFDbContext _ctx;

        public TodoesController()
        {
            var options = new DbContextOptionsBuilder<EFDbContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;

            _ctx = new EFDbContext(options);
            _ctx.Todoes.Add(new Todo
            {
                Id = "18kxytxr0cak4ay1nj4d",
                Title = "18kxytxr0cak4ay1nj4d",
            });
            _ctx.Add(new Todo
            {
                Id = "228kxytxr0cak4ay1nj4d",
                Title = "228kxytxr0cak4ay1nj4d",
            });
            _ctx.SaveChanges();
        }

        // GET: TodoesController1
        public ActionResult Index()
        {
            var Todoes = _ctx.Todoes;
            return View(Todoes);
        }

    }
}
EOF
cd ..
mkdir Views/Todoes

cd ./Views/Todoes/
cat > Index.cshtml << EOF
@model IEnumerable<$Root.Classlib.Entities.Todo>
<div>
  @foreach (var item in Model)
  {
    <div>@item.ToString()</div>

  }
</div>
EOF
cd ../../..
