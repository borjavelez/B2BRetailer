using EasyNetQ;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Warehouse
{
    public class Warehouse
    {
        private int id;
        private string country;
        private List<Product> products;

        public Warehouse(int id, string country, List<Product> products)
        {
            this.id = id;
            this.country = country;
            this.products = products;
        }


        public void Start()
        {
            //Subscribe
            Console.Title = "Warehouses";
            var country = this.country;
            var products = this.products;

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                Console.WriteLine("Listening for products. Hit <return> to quit.");

                bus.Subscribe<Order>("subscriber" + id, a => HandleOrderReplyMessage(a, id, country, products, bus), x => x.WithTopic(country).WithTopic("all"));

                Console.ReadLine();
            }
        }
        
        public static void HandleOrderReplyMessage(Order order, int id, string country, List<Product> products, IBus bus)
        {
            bool productFound = false;
            for (int i = 0; i < products.Count; i++)
            {
                if (order.Product.Id.Equals(products[i].Id))
                {
                    productFound = true;
                    if (country.Equals(order.Customer.origin))
                    {
                        order.DeliveryDays = 2;
                        order.ShippingCost = 5;
                        order.TotalCost = products[i].Price + order.ShippingCost;
                        order.Product.Price = products[i].Price;
                        order.Warehouse = country;
                        break;
                    }
                    else
                    {
                        order.DeliveryDays = 10;
                        order.ShippingCost = 10;
                        order.TotalCost = products[i].Price + order.ShippingCost;
                        order.Warehouse = country;
                        order.Product.Price = products[i].Price;
                        break;
                    }
                }
            }
            Console.WriteLine("Product not found");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Warehouse" + country + " => Received from Retailer: " + order.Id);
            Console.WriteLine("-------------");
            Console.ResetColor();

            //Update if the product has been found
            order.ProductFound = productFound;

            //Send the message to the warehouse in a different queue
            bus.Send<Order>("warehouseSendQueue", order);
        }
    }
}
