using EasyNetQ;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;


namespace MessagingGateway
{
    class SynchronousMessagingGateway
    {
        IBus bus = RabbitHutch.CreateBus("host=localhost");
        //TextMessage replyMessage = null;
        int timeout = 5000;

        public SynchronousMessagingGateway()
        {
            waitForResponse();

        }

        static void send(int productId, String country)
        {
            Customer customer = new Customer("DK");

            Console.WriteLine("send");
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                Product product = new Product { Id = productId };

                bus.Send<Order>("customerSendProductQueue", new Order
                {
                    Customer = customer,
                    Product = product
                });
            }
        }

        static void waitForResponse()
        {
            Console.WriteLine("response");
            //Wait the response from the Retailer
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Receive<Order>("retailerSendQueueToCustomer", responseFromWarehouse => HandleResponseFromRetailer(responseFromWarehouse));
                //Console.ReadLine();
                Thread.Sleep(10000);
            }
        }


        static void HandleResponseFromRetailer(Order order)
        {
            //throw new Exception("Testing invalid message channel");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Customer => Received from Retailer: " + order.Id);
            Console.ResetColor();
            Console.WriteLine("finish");
        }
    }
}
