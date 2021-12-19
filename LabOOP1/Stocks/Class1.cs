using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class Stocks
    {
        //private Dictionary<FoodTypes, int> _foodStocks = new()
        //{
        //    { FoodTypes.meat, 0 },
        //    { FoodTypes.fruit, 0 },
        //    { FoodTypes.plant, 0 }
        //};
        public static bool CheckStoksFullness(Dictionary<FoodTypes, int> _foodStocks, int count = Constants.MaxCountOfFoodStock)
        {
            foreach (var pair in _foodStocks)
            {
                if (CheckStockNotReachedLimit(_foodStocks, pair.Key, count))
                    return false;
            }
            return true;
        }
        public static bool CheckStockNotReachedLimit(Dictionary<FoodTypes, int> _foodStocks, FoodTypes ft, int count = Constants.MaxCountOfFoodStock)
        {
            return _foodStocks[ft] < count;
        }

        public static bool CheckStocks(Dictionary<FoodTypes, int> _foodStocks, FoodTypes food = FoodTypes.any)
        {
            if (food == FoodTypes.any)
            {
                foreach (var pair in _foodStocks)
                {
                    if (pair.Value != 0)
                        return true;
                }
            }
            else
            {
                return _foodStocks[food] != 0;
            }

            return false;
        }

        public static KeyValuePair<FoodTypes, int> FindMaxValueAndItsKey(Dictionary<FoodTypes, int> _foodStocks)
        {
            return _foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
        }
    }
}
