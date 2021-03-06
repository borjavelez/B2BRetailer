﻿using EasyNetQ;
using EasyNetQ.Management.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using EasyNetQ.Management.Client.Model;

namespace MessagingGateway
{
    public class SynchronousMessagingGateway
    {

        IBus bus = RabbitHutch.CreateBus("host=localhost");
        int timeout = 5000;
        Order order = null;
        Customer customer;

        public SynchronousMessagingGateway(Customer customer)
        {
            bus.Subscribe<Order>("subscriber" + customer.Id, a => HandleResponseFromRetailer(a), x => x.WithTopic(customer.Id.ToString()));
        }

        public Order send(int productId, String country, Customer customer)
        {
            this.customer = customer;
            customer.origin = country;
            Product product = new Product { Id = productId };

            Order order = new Order { Customer = customer, Product = product };

            bus.Send<Order>("customerSendProductQueue", order);

            bool gotReply;

            lock (this)
            {
                gotReply = Monitor.Wait(this, timeout);
            }

            if (gotReply)
            {
                return this.order;
            }
            else
            {
                throw new Exception("Timeout!");
            }
        }

        void HandleResponseFromRetailer(Order order)
        {
            lock (this)
            {
                Monitor.Pulse(this);
            }
            this.order = order;

        }

        public void Close()
        {
            if (bus != null)
                bus.Dispose();
        }

        public void deleteQueues()
        {
            ManagementClient m = new ManagementClient("http://localhost", "guest", "guest");
            var queues = m.GetQueues();
            foreach (Queue queue in queues)
            {
                m.DeleteQueue(queue);
            }
        }

    }
}
