using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        // Assign the random class to a `Program` property.
        public static Random GenerateRandomNumber = new Random();

        static void Main(string[] args)
        {
            // Create list which will contain the `filteredProductList` Dictionary
            var productList = new List<KeyValuePair<string, string>>();

            // Parse the CSV using the `ParseCSV` function and return its values
            var productPropertyList = ParseCSV("C:/Users/KCA4/devel/Ergasia/products.csv");

            // Add `productPropertyList` Dictionary entries to the `productList` List
            // This is used to pick random products for later on
            foreach (var productAttributes in productPropertyList) {
                productList.Add(productAttributes);
            }
            
            // Create customers and their order lists
            var customer1 = new Customer("Giorikas");
            var customer2 = new Customer("Kostikas");

            var customer1OrderList = new List<Order>();
            var customer2OrderList = new List<Order>();

            // Populate the customer's orders and assign a delivery address
            var customer1Order = PopulateOrder(productList, "Kilkis 34");
            var customer2Order = PopulateOrder(productList, "Thessaloniki 3");

            // Add orders to lists
            customer1OrderList.Add(customer1Order);
            customer2OrderList.Add(customer2Order);

            // Set `Customer.Order` = to the customer's order list
            customer1.Orders = customer1OrderList;
            customer2.Orders = customer2OrderList;

            // Write which customer is more valueable based on `OrderAmount`
            Console.WriteLine(
                    customer1Order.OrderAmount > customer2Order.OrderAmount ?
                    $@"Customer {customer1.LastName} is the more valueable customer " +
                    $"({customer1Order.OrderAmount} > {customer2Order.OrderAmount})" 
                    : 
                    $@"Customer {customer2.LastName} is the more valueable customer" +
                    $"({customer2Order.OrderAmount} > {customer1Order.OrderAmount})"
                );

            // Create Dictionary and check product popularity based on 
            // times it has appeared on the order lists. 
            var mostValueableProducts = new Dictionary<string, int>();

            mostValueableProducts = CheckProductPopularity(
                mostValueableProducts, customer1Order);
            mostValueableProducts = CheckProductPopularity(
                mostValueableProducts, customer2Order);

            // Sort Dictionary based on value
            var sorted = mostValueableProducts.ToList();

            var sortedMVProductList = sorted
                .OrderByDescending(x => x.Value)
                .Take(10);

            // Print 10 most valueable products
            var i = 1;

            foreach(var mvp in sortedMVProductList) {
                Console.WriteLine($"{i}. {mvp.Key}: Sales {mvp.Value}");
                i++;
            }
            Console.ReadLine();
        }

        public static Dictionary<string, string> ParseCSV(string csvURL)
        {

            if (string.IsNullOrWhiteSpace(csvURL)) {
                throw new ArgumentNullException($"{nameof(csvURL)} can't be null");
            }

            // Open products.csv and parse its lines to the `Lines` variable
            var Lines = File.ReadAllLines($@"{csvURL}");

            // Create new dictionary for both product properties
            var productPropertyList = new Dictionary<string, string>();

            // Parse each line using the variable `line` to the `filteredProductList` Dictionary
            // This ensures that Dictionary keys are Unique. Keys are also Trimmed to ensure
            // there are no bugs due to spaces.
            foreach (var line in Lines) {

                var productProperties = line.Split(';');

                // Create a random price from 1-1000 with decimals rounded up 
                // to the second decimal number. Run the `randomPrice` variable
                // through the `TwoDecimals` recursive function to ensure 
                // there are 2 decimals
                var randomPrice = (double)GenerateRandomNumber.Next(1, 1000) +
                Math.Round(GenerateRandomNumber.NextDouble(), 2);

                randomPrice = TwoDecimals(randomPrice);

                // Add the values to the `filteredProductList` Dictionary.
                // This ensures that all keys are Unique.
                // Prices are stores after the `||` characters to be retrieved later
                productPropertyList[productProperties[0].ToString().Trim()] =
                productProperties[1].ToString() + "|" + randomPrice.ToString();
            }
            return productPropertyList;
        }

        /// <summary>
        /// Recursive function to check if there are two decimals in a random number used
        /// as the product's price. If two decimal numbers aren't present
        /// a new number is generated
        /// </summary>
        /// <param name="randomPrice"></param>
        /// <returns></returns>
        public static double TwoDecimals(double randomPrice)
        {
            var verifyTwoDecimals = randomPrice.ToString().Split('.');

            if (verifyTwoDecimals.ElementAtOrDefault(1) == null) {
                randomPrice = (double)GenerateRandomNumber.Next(1, 1000) +
                Math.Round(GenerateRandomNumber.NextDouble(), 2);

                TwoDecimals(randomPrice);
            }
            return randomPrice;
        }

        /// <summary>
        /// Fill an order with products from a provided `productList`.
        /// The address is also passed as a parameter
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        public static Order PopulateOrder (
            List<KeyValuePair<string, string>> productList, 
            string deliveryAddress
        )
        {
            if(productList == null) {
                throw new ArgumentNullException($"{nameof(productList)} can't be null");
            }

            var order = new Order();

            // Get `productList` List size
            var productListSize = productList.Count();

            for (var i = 0; i < 10; i++) {
                
                // Grab a random entry from the provided `productList`
                var randomListEntry = productList[GenerateRandomNumber
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

                // Wait 0.1s for more randomness
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