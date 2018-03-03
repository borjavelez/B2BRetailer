using EasyNetQ;
using Models;
using System;
using System.Collections.Generic;

namespace Retailer
{
    class Retailer
    {

        static void Main(string[] args)
        {
            //Meter una lista de orders id que hayan llegado asociado a cada customer para manejar diferentes peticiones de varios customers.
            List<int> pendingOrders = new List<int>();

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                Console.Title = "Retailer";
                bus.Receive<Order>("customerSendProductQueue", receivedOrder => HandleOrderReplyMessage(1, receivedOrder, bus, pendingOrders));
                Console.ReadLine();
            }
        }

        static void processWarehouseResponse(EasyNetQ.IBus bus, Order order, List<int> pendingOrders)
        {
            if (pendingOrders.Contains(order.Id))
            {
                if (order.ProductFound == true) //We have found the product, so we send the response to the customer.
                {
                    bus.Send<Order>("retailerSendQueueToCustomer", order);
                    pendingOrders.Remove(order.Id);
                }
                else //The product has not been found, so we publish to all the warehouses without topic.
                {
                    using (var busNew = RabbitHutch.CreateBus("host=localhost"))
                    {
                        busNew.Publish(order, "all");
                    }
                }
            }

        }

        static void publish(EasyNetQ.IBus bus, Order order)
        {
            bus.Publish(order, order.Customer.origin);
        }

        static void HandleOrderReplyMessage(int queue, Order order, EasyNetQ.IBus bus, List<int> pendingOrders)
        {
            //throw new Exception("Testing invalid message channel");
            switch (queue)
            {

                //Receives message from Customer and publishes to Warehouse
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("CustomerID (" + order.Customer.Id + ") => OrderID: " + order.Id);
                    Console.ResetColor();
                    publish(bus, order);
                    bus.Receive<Order>("warehouseSendQueue", orderFromWarehouse => HandleOrderReplyMessage(2, orderFromWarehouse, bus, pendingOrders));
                    pendingOrders.Add(order.Id);
                    break;

                //Receives message from Warehouse and sends it back to the Customer
                case 2:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Retailer => Received from Warehouse: " + order.Id);
                    Console.WriteLine("Delivery Days" + order.DeliveryDays);
                    Console.ResetColor();
                    processWarehouseResponse(bus, order, pendingOrders);
                    break;
            }
        }
    }
}