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
        private IDocumentCollection<User> _Users;

        public UsersController(IDocumentCollection<User> users)
        {            
            _Users = users;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _Users.All().Select(h => h.Data);
            return View(users);
        }
        public IActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var newUser = _Users.New();            
            newUser.Data.Name = user.Name;
            newUser.Data.LastName = user.LastName;
            newUser.Save().Wait();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            return View(_Users.All().First(e => e.Data.ID == id).Data);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteComfirm(Guid id)
        {
            var user = _Users.All().First(e => e.Data.ID == id);
            user.Delete().Wait();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            return View(_Users.All().First(e => e.Data.ID == id).Data);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            var curr = _Users.All().First(e => e.Data.ID == user.ID);
            curr.SyncUpdate(h => { h.Name = user.Name; h.LastName = user.LastName; }).Wait();            
            return RedirectToAction("Index");
        }
    }
}
