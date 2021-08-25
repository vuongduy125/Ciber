using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.Models
{
    public class CreateOrderModel
    {
        public IEnumerable<SelectListItem> listProduct { get; set; }

        public IEnumerable<SelectListItem> listCustomer { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "OrderName is required")]
        public string OrderName { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int? Customer { get; set; }

        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime? OrderDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [Required(ErrorMessage = "Amount is required")]
        public int? Amount { get; set; }
    }
}
