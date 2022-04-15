using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class Stocks
    {
        public static bool CheckStoksFullness(ref Dictionary<FoodTypes, int> foodStocks, int count = Constants.MaxCountOfFoodStock)
        {
            foreach (var pair in foodStocks)
            {
                if (CheckStockNotReachedLimit(ref foodStocks, pair.Key, count))
                    return false;
            }
            return true;
        }
        public static bool CheckStockNotReachedLimit(ref Dictionary<FoodTypes, int> foodStocks, FoodTypes ft, int count = Constants.MaxCountOfFoodStock)
        {
            return foodStocks[ft] < count;
        }

        public static bool CheckIfAtLastOneLimit(ref Dictionary<FoodTypes, int> foodStocks, int count = Constants.MaxCountOfFoodStock)
        {
            foreach (var pair in foodStocks)
            {
                if (!CheckStockNotReachedLimit(ref foodStocks, pair.Key, count))
                    return true;
            }
            return false;
        }
        public static void GetOneItem(ref Dictionary<FoodTypes, int> foodStocks, FoodTypes ft)
        {
            foodStocks[ft]--;   
        }
        public static void PutFood(ref Dictionary<FoodTypes, int> foodStocks, FoodTypes ft, int count)
        {
            foodStocks[ft] += count;
        }

        public static bool CheckStocksContainFoodType(ref Dictionary<FoodTypes, int> foodStocks, FoodTypes food = FoodTypes.any)
        {
            if (food == FoodTypes.any)
            {
                foreach (var pair in foodStocks)
                {
                    if (pair.Value > 0)
                        return true;
                }
                return false;
            }

            return foodStocks[food] > 0;
        }

        public static KeyValuePair<FoodTypes, int> FindMaxValueAndItsKey(ref Dictionary<FoodTypes, int> foodStocks)
        {
            return foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
        }
    }
}
