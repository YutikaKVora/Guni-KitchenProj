﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guni_Kitchen.Data;
using Guni_Kitchen.Models;
using Guni_Kitchen.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Azure.Storage.Blobs;

namespace Guni_Kitchen.Controllers
{
    public class ProductsController : Controller
    {
        private const string BlobContainerNAME = "productimages";

        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, IConfiguration config, ILogger<ProductsController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,Price,UnitOfMeasure,Size,CategoryId, Photo")] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                string photoUrl = null;
                if(productViewModel.Photo != null)
                {
                    photoUrl = await SavePhotoToBlobAsync(productViewModel.Photo);
                }
                Product newProduct = new Product
                {
                    ProductId = productViewModel.ProductId,
                    CategoryId = productViewModel.CategoryId,
                    ProductName = productViewModel.ProductName,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    UnitOfMeasure = productViewModel.UnitOfMeasure,
                    Size = productViewModel.Size,
                    ProductImageFileUrl = photoUrl,
                    ProductImageContentType = photoUrl == null ? null: productViewModel.Photo.ContentType
                };

                _context.Add(newProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", productViewModel.CategoryId);
            return View(productViewModel);
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
            var productViewModel = new ProductViewModel
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                UnitOfMeasure = product.UnitOfMeasure,
                Size = product.Size,
            };
            ViewBag.ProductImageFileUrl = product.ProductImageFileUrl;
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", productViewModel.CategoryId);
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,UnitOfMeasure,Size,CategoryId,Photo")] ProductViewModel productViewModel)
        {
            if (id != productViewModel.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string photoUrl = null;
                if (productViewModel.Photo != null)
                {
                    // Upload the product image to the Blob Storage Account.
                    photoUrl = await SavePhotoToBlobAsync(productViewModel.Photo);
                }

                // Get the product to update, and update its properties.
                var product = _context.Products.SingleOrDefault(p => p.ProductId == productViewModel.ProductId);
                product.CategoryId = productViewModel.CategoryId;
                product.ProductName = productViewModel.ProductName;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.UnitOfMeasure = productViewModel.UnitOfMeasure;
                product.Size = productViewModel.Size;
                if (photoUrl != null)
                {
                    product.ProductImageFileUrl = photoUrl;
                    product.ProductImageContentType = productViewModel.Photo.ContentType;
                }
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", productViewModel.CategoryId);
            return View(productViewModel);
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
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        private async Task<string> SavePhotoToBlobAsync(IFormFile productImage)
        {
            string storageConnection1 = _config.GetValue<string>("MyAzureSettings:StorageAccountKey1");
            string storageConnection2 = _config.GetValue<string>("MyAzureSettings:StorageAccountKey2");
            string fileName = productImage.FileName;
            string tempFilePath = string.Empty;
            string photoUrl;

            if (productImage != null && productImage.Length > 0)
            {
                // Save the uploaded file on to a TEMP file.
                tempFilePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    productImage.CopyToAsync(stream).Wait();
                }
            }

            // Get a reference to a container 
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnection1, BlobContainerNAME);

            // Create the container if it does not exist - granting PUBLIC access.
            await blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            // Create the client to the Blob Item
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            // Open the file and upload its data
            using (FileStream uploadFileStream = System.IO.File.OpenRead(tempFilePath))
            {
                await blobClient.UploadAsync(uploadFileStream, overwrite: true);
                uploadFileStream.Close();
            }

            // Delete the TEMP file since it is no longer needed.
            System.IO.File.Delete(tempFilePath);

            // Return the URI of the item in the Blob Storage
            photoUrl = blobClient.Uri.ToString();
            return photoUrl;
        }

    }
}
