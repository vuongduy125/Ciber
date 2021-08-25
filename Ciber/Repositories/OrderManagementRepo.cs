using Ciber.CiberDbContext;
using Ciber.Interface;
using Ciber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.Repositories
{
    public class OrderManagementRepo : IOrderManagementRepo
    {
        private readonly CiberOrderDbContext _dbContext;
        private int _totalOrder;
        private int _pageCount;
        private readonly int _limit = 2;

        public OrderManagementRepo(CiberOrderDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<OrdersDisplay> ListingOrder(string searchString, int start, int limit)
        {
            IQueryable<OrdersDisplay> orders = from product in _dbContext.Product
                                               join order in _dbContext.Order
                                               on product.ProductId equals order.ProductId
                                               join customer in _dbContext.Customer
                                               on order.CustomerId equals customer.CustomerId
                                               join category in _dbContext.Category
                                               on product.CategoryId equals category.CategoryId
                                               where string.IsNullOrEmpty(searchString) || product.Name.Contains(searchString)
                                               select new OrdersDisplay()
                                               {
                                                   CategoryName = category.Name,
                                                   CustomerName = customer.Name,
                                                   Description = product.Description,
                                                   Name = product.Name,
                                                   Price = product.Price,
                                                   Amount = order.Amount,
                                                   ProductId = product.ProductId,
                                                   OrderDate = order.OrderDate,
                                                   OrderId = order.OrderId
                                               };

            _totalOrder = orders.Count();

            orders = orders.OrderByDescending(o => o.OrderId).Skip(start).Take(limit);
            
            return orders;
        }

        public int GetTotalOrder()
        {
            return _totalOrder;
        }

        public int GetNumberPage(int limit)
        {
            float numberpage = (float)_totalOrder / limit;
            return (int)Math.Ceiling(numberpage);
        }

        public int GetAvailableProductQuantity(CreateOrderModel orderModel)
        {
            var orderedQuantity = _dbContext.Product.Where(o => o.ProductId == orderModel.ProductId).FirstOrDefault().Quantity;
            return orderedQuantity;
        }

        public void CreateOrderUpdateProduct(CreateOrderModel orderModel)
        {
            Order order = new Order();
            order.CustomerId = (int)orderModel.Customer;
            order.ProductId = (int)orderModel.ProductId;
            order.Amount = (int)orderModel.Amount;
            order.OrderDate = (DateTime)orderModel.OrderDate;
            _dbContext.Add(order);

            var product = _dbContext.Product.Where(o => o.ProductId == orderModel.ProductId).FirstOrDefault();
            product.Quantity = product.Quantity - (int)orderModel.Amount;
            _dbContext.Update(product);

            _dbContext.SaveChanges();
        }
    }
}
