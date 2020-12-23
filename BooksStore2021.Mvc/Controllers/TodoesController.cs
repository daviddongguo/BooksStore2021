using BooksStore2021.Classlib.Entities;
using BooksStore2021.Classlib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksStore2021.Mvc.Controllers
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
