using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class Building : MapObject
    {
        public (int, int) currentPosition;
        public int indexOfVillage;

        internal Dictionary<FoodTypes, int> foodStocks = new()
        {
            { FoodTypes.meat, 0 },
            { FoodTypes.fruit, 0 },
            { FoodTypes.plant, 0 }
        };
        public Building((int, int) pos)
        {
            currentPosition = pos;
        }
        protected abstract string GetInfo();

        
        internal bool CheckStocksInHouse(FoodTypes ft = FoodTypes.any)
        {
            return Stocks.CheckStocksContainFoodType(ref foodStocks, ft);
        }

        internal (int, int) GetPosition()
        {
            return currentPosition;
        }
        public override string GetInfoAndLight()
        {
            Rendering.LightChoosen(currentPosition.Item1, currentPosition.Item2);
            return GetInfo();
        }
    }
}
