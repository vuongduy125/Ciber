using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ciber.Models;
using Ciber.CiberDbContext;
using Ciber.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Ciber.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderManagementRepo _iOrderManagementRepo;
        CiberOrderDbContext _dbContext = new CiberOrderDbContext();

        public HomeController(IOrderManagementRepo iOrderManagementRepo)
        {
            this._iOrderManagementRepo = iOrderManagementRepo;
        }

        public IActionResult Index(
            string searchString
            , int? page = 0)
        {
            int limit = 3;
            int start;
            page = page > 0 ? page : 1;
            start = (int)(page - 1) * limit;

            ViewBag.currentPage = page;
            IEnumerable<OrdersDisplay> orders = _iOrderManagementRepo.ListingOrder(searchString, start, limit);
            int totalOrder = _iOrderManagementRepo.GetTotalOrder();

            ViewData["CurrentFilter"] = searchString;
            ViewBag.totalOrder = totalOrder;
            ViewBag.numberPage = _iOrderManagementRepo.GetNumberPage(limit);

            return View(orders);
        }

        public IActionResult _CreateOrderModal()
        {
            CreateOrderModel createOrder = new CreateOrderModel();
            createOrder.listProduct = _dbContext.Product.Select(o => new SelectListItem
            {
                Value = o.ProductId.ToString(),
                Text = o.Name
            });

            createOrder.listCustomer = _dbContext.Customer.Select(o => new SelectListItem
            {
                Value = o.CustomerId.ToString(),
                Text = o.Name
            });

            return PartialView("_CreateOrderModal", createOrder);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderModel orderModel)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string errorMessage = null;
                    if (!ModelState.IsValid)
                    {
                        foreach (var key in ViewData.ModelState.Keys)
                        {
                            foreach (var e in ViewData.ModelState[key].Errors)
                            {
                                errorMessage += e.ErrorMessage + "<br />";
                            }
                        }

                        return Json(new { result = 1, errmsg = errorMessage });
                    }
                    else
                    {
                        // Validate Quantity
                        int availableQuantity = _iOrderManagementRepo.GetAvailableProductQuantity(orderModel);

                        if (availableQuantity < orderModel.Amount)
                        {
                            errorMessage += "Avaiable quantity is " + availableQuantity.ToString();
                            return Json(new { result = 1, errmsg = errorMessage });
                        }

                        _iOrderManagementRepo.CreateOrderUpdateProduct(orderModel);

                        transaction.Commit();
                        return Json(new { result = 2, errmsg = "" });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { result = 3, errmsg = ex.Message });
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
