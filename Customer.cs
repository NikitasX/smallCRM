using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Customer : Person
    {
        public Customer(string lastname) : base(lastname) 
        {
            Age = 20;
            Orders = new List<Order>();
        }
        public int CustomerID { get; set; }
        public List<Order> Orders;
    }
}
