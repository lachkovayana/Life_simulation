using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class House : MapObject
    {
        public (int, int) currentPosition;
        public Human MaleOwner;
        public Human FemaleOwner;
        public string ReasonForBuilding;
        public int indexOfVillage;

        private Dictionary<FoodTypes, int> _foodStocks = new()
        {
            { FoodTypes.meat, 0 },
            { FoodTypes.fruit, 0 },
            { FoodTypes.plant, 0 }
        };

        public House((int, int) pos, (int, int, int) baseHouseData)
        {
            currentPosition = pos;

            ReasonForBuilding = baseHouseData == default ? "random" :
                "pos: (" + baseHouseData.Item1.ToString() + ", " +
                baseHouseData.Item2.ToString() + ") and " +
                baseHouseData.Item3.ToString() + " village.";
        }

        internal void PutFood(FoodTypes ft, int count)
        {
            _foodStocks[ft] += count;
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

        private string GetInfo()
        {
            var linesS = _foodStocks.Select(kvp => "- " + kvp.Key + ": " + kvp.Value + "/" + Constants.MaxCountOfFoodStock);

            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();

            string owners = "\r\nOwners: male " + MaleOwner.GetPosition().ToString() + ", female " + FemaleOwner.GetPosition().ToString() + "\r\n";

            string result = string.Concat("It's a ", name,
                " with position ", currentPosition, "\r\nStocks inside :\r\n", string.Join(Environment.NewLine, linesS), owners);

            result += "\r\n";
            result += "Was built based on " + ReasonForBuilding;
            result += "\r\n\r\n";
            result += "Current index of village: " + indexOfVillage;

            return result;
        }


    }
}
