using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.ResponeModel;
using FunitureApp.Areas.admin.Models;

namespace FunitureApp.Areas.admin.Controllers
{
    [Area("admin")]
    public class UserOrdersController : Controller
    {
        private readonly DbFunitureContext _context;

        public UserOrdersController()
        {
            _context = new DbFunitureContext();
        }

        // GET: admin/UserOrders
        public async Task<IActionResult> Index()
        {
            var userOrder = await _context.UserOrders.ToListAsync();
            var response = new List<OrderView>();

            foreach (var o in userOrder)
            {
                var orderRes = new OrderView()
                {
                    Order = o,
                    User = _context.Users.Find(o.User_id)
                };
                
                
                response.Add(orderRes);
            }


            return View(response);
        }

        // GET: admin/UserOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // GET: admin/UserOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/UserOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Order_no,User_id,Total,Status,Delivery_method_id,Delivery_free,Create_at,ShipId,PaymentStatus,PaymentType")] UserOrder userOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userOrder);
        }

        // GET: admin/UserOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders.FindAsync(id);
            if (userOrder == null)
            {
                return NotFound();
            }
            return View(userOrder);
        }

        // POST: admin/UserOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Order_no,User_id,Total,Status,Delivery_method_id,Delivery_free,Create_at,ShipId,PaymentStatus,PaymentType")] UserOrder userOrder)
        {
            if (id != userOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOrderExists(userOrder.Id))
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
            return View(userOrder);
        }

        // GET: admin/UserOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // POST: admin/UserOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userOrder = await _context.UserOrders.FindAsync(id);
            _context.UserOrders.Remove(userOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOrderExists(int id)
        {
            return _context.UserOrders.Any(e => e.Id == id);
        }
    }
}
