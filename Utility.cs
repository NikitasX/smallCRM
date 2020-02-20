using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Ergasia
{
    public class Utility
    {
        // Assign the random class to a `Utility` property.
        public static Random GenerateRandomNumber = new Random();

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
            var verifyTwoDecimals = randomPrice.ToString(CultureInfo.InvariantCulture).Split('.');

            if (verifyTwoDecimals.ElementAtOrDefault(1) == null) {

                randomPrice = (double)GenerateRandomNumber.Next(1, 1000) +
                Math.Round(GenerateRandomNumber.NextDouble(), 2);

                TwoDecimals(randomPrice);
            }
            return randomPrice;
        }
    }
}
