using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BooksStore2021.Mvc.Controllers
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

            if (_db.Todoes.FirstOrDefault() == null)
            {
                _db.Todoes.Add(new Todo
                {
                    Id = "18kxytxr0cak4ay1nj4d",
                    Title = "18kxytxr0cak4ay1nj4d" + DateTime.Now,
                });
                _db.Add(new Todo
                {
                    Id = "228kxytxr0cak4ay1nj4d",
                    Title = "228kxytxr0cak4ay1nj4d" + DateTime.Now,
                    IsComplete = true,
                }); ;
                try
                {
                    _db.SaveChanges();
                }
                catch (System.Exception)
                {
                }
            }
        }

        // GET: TodoesController1
        public ActionResult Index()
        {
            var Todoes = _db.Todoes;
            return View(Todoes);
        }

    }
}
