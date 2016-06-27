using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mocoding.EasyDocDb.CrudSample.Models;

namespace Mocoding.EasyDocDb.CrudSample.Controllers
{
    [AllowAnonymous]
    public class CategoriesController : Controller
    {
        private readonly IDocumentCollection<Category> _categories;

        public CategoriesController(IDocumentCollection<Category> categories)
        {
            _categories = categories;    
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(_categories.All().Select(i => i.Data).ToList());
        }

        // GET: Categories/Details/5
        public IActionResult Details(Guid id)
        {
            var category = _categories.All().FirstOrDefault(m => m.Data.CategoryId == id)?.Data;
            return View(category);
        }

        // GET: Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var newCategory = _categories.New();
                newCategory.Data.CategoryId = category.CategoryId;
                newCategory.Data.CategoryName = category.CategoryName;   
                newCategory.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(Guid id)
        {
            var category = _categories.All().FirstOrDefault(m => m.Data.CategoryId == id)?.Data;
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var currentCategory = _categories.All().FirstOrDefault(m => m.Data.CategoryId == category.CategoryId);
                currentCategory.SyncUpdate(d => d.CategoryName = category.CategoryName);
                //currentCategory.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(Guid id)
        {
            var category = _categories.All().FirstOrDefault(m => m.Data.CategoryId == id);
            return View(category.Data);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var category = _categories.All().FirstOrDefault(m => m.Data.CategoryId == id);                 
            category.Delete();       
            return RedirectToAction("Index");
        }
    }
}
