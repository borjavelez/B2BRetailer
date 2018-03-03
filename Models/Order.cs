using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int? DeliveryDays { get; set; }
        public double? ShippingCost { get; set; }
        public double? TotalCost { get; set; }
        public bool? ProductFound { get; set; }
        public String Warehouse { get; set; }

        public Order()
        {
            Random rnd = new Random();
            this.Id = rnd.Next(1, 9999999);
        }
    }
}
