using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Authorization;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.SimpleSample.Models;
using System;

namespace Mocoding.EasyDocDb.SimpleSample.Controllers
{
    [AllowAnonymous]
    public class CategoriesController : Controller
    {
        private readonly IDocumentCollection<Category> _category;

        public CategoriesController(IDocumentCollection<Category> category)
        {
            _category = category;    
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(_category.All().Select(i => i.Data).ToList());
        }

        // GET: Categories/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return HttpNotFound();
            }

            var category = _category.All().FirstOrDefault(m => m.Data.CategoryId == id)?.Data;
            if (category == null)
            {
                return HttpNotFound();
            }

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
                var newCategory = _category.New();
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
            if (id == Guid.Empty)
            {
                return HttpNotFound();
            }

            var category = _category.All().FirstOrDefault(m => m.Data.CategoryId == id)?.Data;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var currentCategory = _category.All().FirstOrDefault(m => m.Data.CategoryId == category.CategoryId);
                currentCategory.Data.CategoryName = category.CategoryName;
                currentCategory.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = _category.All().FirstOrDefault(m => m.Data.CategoryId == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category.Data);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var category = _category.All().FirstOrDefault(m => m.Data.CategoryId == id);                 
            category.Delete();       
            return RedirectToAction("Index");
        }
    }
}
