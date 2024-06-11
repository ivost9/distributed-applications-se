using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App2.Data;
using App2.Models;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        //POST: Trainings/ShowSearchResults

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Products.Where(j => j.Name.Contains(SearchPhrase))
                .ToListAsync());
        }
        public async Task<IActionResult> ShowSearchResultsCategory(String SearchPhrase)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.Contains(SearchPhrase));

            if (category == null)
            {
                // Ако категорията не е намерена, връщаме празен списък
                return View("Index", new List<Product>());
            }
            else
            {
                // Намираме продуктите, които принадлежат на намерената категория
                var productsInCategory = await _context.Products.Where(p => p.CategoryId == category.Id).ToListAsync();

                return View("Index", productsInCategory);
            }
        }
        // GET: Products

        //public async Task<IActionResult> Index(String SearchPhrase)
        //{

        //        var appDbContext = _context.Products.Include(p => p.Category);
        //        return View(await appDbContext.ToListAsync());


        //}
        public async Task<IActionResult> Index(string SearchPhrase, string searchBy)
        {
            if (string.IsNullOrEmpty(SearchPhrase) || string.IsNullOrEmpty(searchBy))
            {
                // Ако няма търсене, връщаме всички продукти
                var appDbContext = _context.Products.Include(p => p.Category);
                return View(await appDbContext.ToListAsync());
            }

            if (searchBy == "name")
            {
                // Търсене по име
                return View("Index", await _context.Products.Where(j => j.Name.Contains(SearchPhrase)).ToListAsync());
            }
            else if (searchBy == "category")
            {
                // Търсене по категория
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.Contains(SearchPhrase));

                if (category == null)
                {
                    // Ако категорията не е намерена, връщаме празен списък
                    return View("Index", new List<Product>());
                }
                else
                {
                    // Намираме продуктите, които принадлежат на намерената категория
                    var productsInCategory = await _context.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
                    return View("Index", productsInCategory);
                }
            }
            else
            {
                // Ако няма ясно зададен начин на търсене, връщаме всички продукти
                var appDbContext = _context.Products.Include(p => p.Category);
                return View(await appDbContext.ToListAsync());
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (!ModelState.IsValid)
            {
                if (product.Price<0)
                {
                    throw new ArgumentOutOfRangeException("Цената трябва да бъде положително число!");
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (product.Price < 0)
                {
                    throw new ArgumentOutOfRangeException("Цената трябва да бъде положително число!");
                }
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
