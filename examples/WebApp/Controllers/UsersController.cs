using System;
using System.Linq;
using System.Threading.Tasks;
using EasyDocDb.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace EasyDocDb.WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly IDocumentCollection<User> _users;

        public UsersController(IDocumentCollection<User> users)
        {
            _users = users;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _users.Documents.Select(h => h.Data);
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var newUser = _users.New();
            newUser.Data.Name = user.Name;
            newUser.Data.LastName = user.LastName;
            await newUser.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            return View(_users.Documents.First(e => e.Data.Id == id).Data);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteComfirm(Guid id)
        {
            var user = _users.Documents.First(e => e.Data.Id == id);
            await user.Delete();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            return View(_users.Documents.First(e => e.Data.Id == id).Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            var curr = _users.Documents.First(e => e.Data.Id == user.Id);
            await curr.SyncUpdate(h =>
            {
                h.Name = user.Name;
                h.LastName = user.LastName;
            });
            return RedirectToAction("Index");
        }
    }
}