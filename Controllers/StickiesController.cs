using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stickies.Models;

namespace Stickies.Controllers
{
    public class StickiesController : Controller
    {
        private readonly StickyContext _context;

        public StickiesController(StickyContext context)
        {
            _context = context;
        }

        // GET: Stickies
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScreenSticky.ToListAsync());
        }

        public string SaveNewOrder(string Ids) {
            // do something here to reorder stuff
            // I need to get in something like 1, 2, 3 and turn it into an array
            // How can this be?
            List<int> stickyIds = ConvertNumbers(Ids);

            // Get one of the stickies
            // Give it its new order
            // Save it
            for(int i = 0; i < stickyIds.Count; i++)
            {
                ScreenSticky currentSticky = _context.ScreenSticky.Find(stickyIds[i]);
                currentSticky.Order = i;
            }
            _context.SaveChanges();

            return "I have saved your new order!";
        }

        private List<int> ConvertNumbers(string Numbers)
        {
            string[] IdsAsArray = Numbers.Split(',');
            List<int> Bananas = new List<int>();
            for(int i = 0; i < IdsAsArray.Length; i++)
            {
                Bananas.Add(Convert.ToInt32(IdsAsArray[i]));
            }
            return Bananas;
        }

        // GET: Stickies/Board
        public async Task<IActionResult> Board()
        {
            return View(await _context
                .ScreenSticky
                .OrderBy(x => x.Order)
                .ToListAsync());
        }

        // GET: Stickies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screenSticky = await _context.ScreenSticky
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screenSticky == null)
            {
                return NotFound();
            }

            return View(screenSticky);
        }

        // GET: Stickies/Create
        public IActionResult Create()
        {
            return View();
        }

        // [HttpPost]
        // example other action we could make the form go to?
        // public string TestCreate(string Text) {
        //     return "hello world";
        // }

        // POST: Stickies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Order")] ScreenSticky screenSticky)
        {
            if (ModelState.IsValid)
            {
                _context.Add(screenSticky);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Board));
            }
            return View(screenSticky);
        }

        // GET: Stickies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screenSticky = await _context.ScreenSticky.FindAsync(id);
            if (screenSticky == null)
            {
                return NotFound();
            }
            return View(screenSticky);
        }

        // POST: Stickies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Order")] ScreenSticky screenSticky)
        {
            if (id != screenSticky.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screenSticky);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreenStickyExists(screenSticky.Id))
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
            return View(screenSticky);
        }

        // GET: Stickies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screenSticky = await _context.ScreenSticky
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screenSticky == null)
            {
                return NotFound();
            }

            return View(screenSticky);
        }

        // POST: Stickies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screenSticky = await _context.ScreenSticky.FindAsync(id);
            _context.ScreenSticky.Remove(screenSticky);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreenStickyExists(int id)
        {
            return _context.ScreenSticky.Any(e => e.Id == id);
        }
    }
}
