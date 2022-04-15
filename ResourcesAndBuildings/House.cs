using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class House : Building
    {
        public Human MaleOwner;
        public Human FemaleOwner;
        public string ReasonForBuilding;
        public bool partOfLargeVillage = false;
        public House((int, int) pos, (int, int, int) baseHouseData) : base (pos)
        {
            ReasonForBuilding = baseHouseData == default ? "random" :
                "pos: (" + baseHouseData.Item1.ToString() + ", " +
                baseHouseData.Item2.ToString() + ") and " +
                baseHouseData.Item3.ToString() + " village.";
        }
        protected override string GetInfo()
        {
            var linesS = foodStocks.Select(kvp => "- " + kvp.Key + ": " + kvp.Value + "/" + Constants.MaxCountOfFoodStockForHouse);

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
