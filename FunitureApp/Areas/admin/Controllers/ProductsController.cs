using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FunitureApp.Data;
using FunitureApp.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
namespace FunitureApp.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductsController : Controller
    {
        private readonly DbFunitureContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductsController(IHttpContextAccessor httpContext)
        {
            _contextAccessor = httpContext;
            _context = new DbFunitureContext();
        }

        // GET: admin/Products
        public async Task<IActionResult> Index()
        {
            if (_contextAccessor.HttpContext.Session.GetString("admin") != "admin")
                return View("~/Areas/admin/Views/Login.cshtml");
            var data = from p in _context.Products
                       join cate in _context.Categories on p.Category_id equals cate.Id
                       select new Product
                       {
                           Id = p.Id,
                           Category_id = cate.Id,
                           Create_at = p.Create_at,
                           Desc = p.Desc,
                           Image = p.Image,
                           ImageList = p.ImageList,
                           NameProduct = p.NameProduct,
                           Quantity = p.Quantity,
                           Status = p.Status,
                           Type = cate.Name

                       };
            return View(await data.ToListAsync());
        }

        // GET: admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }
         
        // POST: admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public async Task<IActionResult> Create([Bind("NameProduct,Category_id,Image,Desc,Status,Quantity,Create_at,ImageList")] Product product)
        {
            product.Create_at = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: admin/Products/Edit/5
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
            return View(product);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameProduct,Category_id,Image,Desc,Status,Type,Quantity,Create_at,ImageList")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(product);
        }

        // GET: admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> UploadAssets([FromForm] List<IFormFile> files)

        {
            try
            {
                if (files.Any())
                    if (files[0].Length > 0)
                    {
                        var result = new List<string>();
                        foreach (var file in files)
                        {
                            var ext = Path.GetExtension(file.FileName);
                            var absoluteDir = Directory.GetCurrentDirectory();
                            var hostName = Request.Host;
                            var relativeDir = "/images/" +
                        Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = absoluteDir + "/wwwroot" + relativeDir;
                            //savefile
                            //var resultf = await _fileService.SaveFile(filePath, files[0], relativeDir);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            result.Add(relativeDir);
                        }
                        return Ok(result);
                    }
                return StatusCode(400);
            }
            catch (Exception err)
            {
                return StatusCode(500);
            }



        }

        public async Task<IActionResult> GetAttributes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductAttributes.Where(p => p.Product_id == id).ToListAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> SaveAttribute(int price, string color, string hexColor, int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }
            _context.ProductAttributes.Add(new ProductAttribute()
            {
                Color = color,
                HexColor = hexColor,
                Create_at = DateTime.Now,
                Price = price,
                Product_id = productId ?? 0
            });
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> GetCategory(int? productId)
        {
            var category = await _context.Categories.ToListAsync();
            return Ok(category);
        }



    }
}
