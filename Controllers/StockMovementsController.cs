using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeuErp.Data;
using MeuErp.Models;

using Microsoft.AspNetCore.Authorization;

namespace MeuErp.Controllers
{
    [Authorize]
    public class StockMovementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockMovementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StockMovements.Include(s => s.Product).OrderByDescending(s => s.MovementDate);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Type,Quantity,MovementDate,Description")] StockMovement stockMovement)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(stockMovement.ProductId);
                if (product != null)
                {
                    if (stockMovement.Type == MovementType.Exit && product.Quantity < stockMovement.Quantity)
                    {
                        ModelState.AddModelError("Quantity", "Quantidade insuficiente em estoque para esta saída.");
                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stockMovement.ProductId);
                        return View(stockMovement);
                    }

                    if (stockMovement.Type == MovementType.Entry)
                        product.Quantity += stockMovement.Quantity;
                    else
                        product.Quantity -= stockMovement.Quantity;

                    _context.Add(stockMovement);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stockMovement.ProductId);
            return View(stockMovement);
        }
        // GET: StockMovements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement == null) return NotFound();

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stockMovement.ProductId);
            return View(stockMovement);
        }

        // POST: StockMovements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Type,Quantity,MovementDate,Description")] StockMovement stockMovement)
        {
            if (id != stockMovement.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var originalMovement = await _context.StockMovements.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                if (originalMovement == null) return NotFound();

                var product = await _context.Products.FindAsync(stockMovement.ProductId);
                if (product != null)
                {
                    // Revert old movement
                    if (originalMovement.Type == MovementType.Entry)
                        product.Quantity -= originalMovement.Quantity;
                    else
                        product.Quantity += originalMovement.Quantity;

                    // Apply new movement
                    if (stockMovement.Type == MovementType.Exit && product.Quantity < stockMovement.Quantity)
                    {
                        ModelState.AddModelError("Quantity", "Quantidade insuficiente em estoque para esta saída considerando a edição.");
                        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stockMovement.ProductId);
                        return View(stockMovement);
                    }

                    if (stockMovement.Type == MovementType.Entry)
                        product.Quantity += stockMovement.Quantity;
                    else
                        product.Quantity -= stockMovement.Quantity;

                    try
                    {
                        _context.Update(stockMovement);
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StockMovementExists(stockMovement.Id)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stockMovement.ProductId);
            return View(stockMovement);
        }

        // GET: StockMovements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var stockMovement = await _context.StockMovements
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockMovement == null) return NotFound();

            return View(stockMovement);
        }

        // POST: StockMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement != null)
            {
                var product = await _context.Products.FindAsync(stockMovement.ProductId);
                if (product != null)
                {
                    // Revert movement
                    if (stockMovement.Type == MovementType.Entry)
                        product.Quantity -= stockMovement.Quantity;
                    else
                        product.Quantity += stockMovement.Quantity;

                    _context.Update(product);
                }
                
                _context.StockMovements.Remove(stockMovement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StockMovementExists(int id)
        {
            return _context.StockMovements.Any(e => e.Id == id);
        }
    }
}
