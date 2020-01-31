using System;
using System.Linq;
using System.Collections.Generic;

namespace Ergasia
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Create list which will contain the `filteredProductList` Dictionary
            var productList = new List<KeyValuePair<string, string>>();

            // Parse the CSV using the `ParseCSV` function and return its values
            var productPropertyList = Utility.ParseCSV("C:/Users/KCA4/devel/Ergasia/products.csv");

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
            var customer1Order = Functions.PopulateOrder(productList, "Kilkis 34");
            var customer2Order = Functions.PopulateOrder(productList, "Thessaloniki 3");

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

            mostValueableProducts = Functions.CheckProductPopularity(
                mostValueableProducts, customer1Order);
            mostValueableProducts = Functions.CheckProductPopularity(
                mostValueableProducts, customer2Order);

            // Sort Dictionary based on value and Dictionary list size to 10
            var sorted = mostValueableProducts.ToList();

            var sortedMVProductList = sorted
                .OrderByDescending(x => x.Value)
                .Take(10);

            // Print 10 most valuable products
            var i = 1;

            foreach(var mvp in sortedMVProductList) {
                Console.WriteLine($"{i}. {mvp.Key}: Sales {mvp.Value}");
                i++;
            }
            Console.ReadLine();
        }
    }
}