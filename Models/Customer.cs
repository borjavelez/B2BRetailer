﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Customer
    {
        public int Id { get; set; }
        public String origin { get; set; }

        public Customer()
        {
            Random rnd = new Random();
            this.Id = rnd.Next(1, 99999);
        }
    }
}
