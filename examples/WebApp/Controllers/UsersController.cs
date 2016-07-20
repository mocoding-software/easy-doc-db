using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.Json;
using EasyDocDb.WebApplication.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDocDb.WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private IDocumentCollection<User> _users;

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
            return View(_users.Documents.First(e => e.Data.ID == id).Data);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteComfirm(Guid id)
        {
            var user = _users.Documents.First(e => e.Data.ID == id);
            await user.Delete();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            return View(_users.Documents.First(e => e.Data.ID == id).Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            var curr = _users.Documents.First(e => e.Data.ID == user.ID);
            await curr.SyncUpdate(h => { h.Name = user.Name; h.LastName = user.LastName; });            
            return RedirectToAction("Index");
        }
    }
}
