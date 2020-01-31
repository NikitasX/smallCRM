using System;
using System.Collections.Generic;
using System.Linq;

namespace Ergasia
{
    class Functions
    {
        /// <summary>
        /// Fill an order with products from a provided `productList`.
        /// The address is also passed as a parameter
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        public static Order PopulateOrder(
            List<KeyValuePair<string, string>> productList,
            string deliveryAddress)
        {
            if (productList == null) {
                throw new ArgumentNullException($"{nameof(productList)} can't be null");
            }

            if(string.IsNullOrWhiteSpace(deliveryAddress)) {
                throw new ArgumentNullException($"{nameof(deliveryAddress)} can't be null");
            }

            var order = new Order();

            // Get `productList` List size
            var productListSize = productList.Count();

            for (var i = 0; i < 10; i++) {

                // Grab a random entry from the provided `productList`
                var randomListEntry = productList[Utility.GenerateRandomNumber
                    .Next(0, productListSize - 1)];

                // Seperate the product description from the price
                var productAttributes = randomListEntry.Value.Split('|');

                // Create the product and assign its attributes from the parsed list
                var product = new Product()
                {
                    ProductID = randomListEntry.Key.ToString(),
                    ProductPrice = decimal.Parse(productAttributes[1]),
                    ProductDescription = productAttributes[0]
                };

                // Add the product to the order's product list
                // and increase the `OrderAmount` by the product's price
                order.DeliveryAdress = deliveryAddress;
                order.OrderAmount += product.ProductPrice;
                order.Products.Add(product);

                // Wait 0.01s for more randomness
                System.Threading.Thread.Sleep(10);
            }
            return order;
        }

        /// <summary>
        /// Check how any times a product key appears in an array and return
        /// the Dictionary.
        /// </summary>
        /// <param name="mostValueableProducts"></param>
        /// <param name="customerOrder"></param>
        /// <returns></returns>
        public static Dictionary<string, int> CheckProductPopularity(
            Dictionary<string, int> mostValueableProducts, Order customerOrder)
        {

            if (mostValueableProducts == null ||
                customerOrder == null) {
                throw new ArgumentNullException($"{mostValueableProducts} and " +
                    $"{customerOrder} cannot be null");
            }

            foreach (var p in customerOrder.Products) {
                if (!mostValueableProducts.ContainsKey(p.ProductID)) {
                    mostValueableProducts[p.ProductID] = 1;
                } else if (mostValueableProducts.ContainsKey(p.ProductID)) {
                    mostValueableProducts[p.ProductID] += 1;
                }
            }
            return mostValueableProducts;
        }
    }
}
