using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork db)
        {
            _unitofwork = db;
        }
        public IActionResult Index()
        {
            List<Category> ObjcategoriesList = _unitofwork.Category.GetAll().ToList();
            return View(ObjcategoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category newcategory)
        {
            if (newcategory.Name == newcategory.DesplayOrder.ToString())
                ModelState.AddModelError("name", "Name and Display Order can not be the same");
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(newcategory);
                _unitofwork.Save();
                TempData["Success"] = "Category Has been Created";
                return RedirectToAction("Index");
            }
            return View(newcategory);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _unitofwork.Category.Get(u => u.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(obj);
                _unitofwork.Save();
                TempData["Success"] = "Category Has been Edited";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            Category? category = _unitofwork.Category.Get(u => u.CategoryId == id);
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Category? obj = _unitofwork.Category.Get(u => u.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.Category.Delete(obj);
            _unitofwork.Save();
            TempData["Success"] = "Category Has been Deleted";
            return RedirectToAction("Index");
        }
    }
}
