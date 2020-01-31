using System.Collections.Generic;

namespace Ergasia
{
    class Order
    {

        public Order ()
        {
            Products = new List<Product>();
        }

        public int OrderID { get; set; }

        public string DeliveryAdress { get; set; }

        public decimal OrderAmount { get; set; }

        public List<Product> Products;
    }
}
