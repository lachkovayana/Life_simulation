using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class House
    {
        private (int, int) pos ;
        private Dictionary<FoodTypes, int> _foodStocks = new()
        {
            { FoodTypes.meat, 0 },
            { FoodTypes.fruit, 0 },
            { FoodTypes.plant, 0 }
        };
        public House((int, int) pos, Dictionary<FoodTypes, int> foodStocks)
        {
            this.pos = pos;
        }

        public void PutFood(FoodTypes ft)
        {
            _foodStocks[ft]++;
        }

        internal (int, int) GetPosition()
        {
            return pos;
        }
    }
}
