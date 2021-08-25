using Ciber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.Interface
{
    public interface IOrderManagementRepo
    {
        IQueryable<OrdersDisplay> ListingOrder(string searchString, int start, int limit);

        int GetTotalOrder();

        int GetNumberPage(int limit);

        int GetAvailableProductQuantity(CreateOrderModel orderModel);

        void CreateOrderUpdateProduct(CreateOrderModel orderModel);
    }
}
