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

        //public static void CheckIfIsLocal(Order order, int id, string country, List<Product> products, IBus bus)
        //{
        //    if (order.Customer.origin.Equals(country))
        //    {
        //        var id2 = int.MaxValue-id;
        //        bus.Subscribe<Order>("subscriber" + id2,
        //            a => HandleOrderReplyMessage(order, id, country, products, bus), x => x.WithTopic(country));
        //    }
        //    else
        //    {
        //        bus.Subscribe<Order>("subscriber" + id, a => HandleOrderReplyMessage(order, id, country, products, bus));
        //    }
        //}

        public static void HandleOrderReplyMessage(Order order, int id, string country, List<Product> products, IBus bus)
        {
            Console.WriteLine("Entrado en Handle");

            bool productFound = false;
            // throw new Exception("Testing invalid message channel");
            //var query = order.Products.Where(o => o.Name == "Model S");
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
                        break;
                    }
                    else
                    {
                        order.DeliveryDays = 10;
                        order.ShippingCost = 10;
                        order.TotalCost = products[i].Price + order.ShippingCost;
                        break;
                    }
                }
            }
            Console.WriteLine("Product not found");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Warehouse" + country + " => Received from Retailer: " + order.Id);
            Console.ResetColor();

            //Update if the product has been found

            order.ProductFound = productFound;
            //Send the message to the warehouse in a different queue

            //using (var bus = RabbitHutch.CreateBus("host=localhost"))
            //{
            bus.Send<Order>("warehouseSendQueue", order);
            //}
        }
    }
}
