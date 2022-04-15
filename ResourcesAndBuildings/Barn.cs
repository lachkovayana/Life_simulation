using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public class Barn : Building
    {
        public Barn((int, int) pos) : base(pos)
        {}
        protected override string GetInfo()
        {
            var linesS = foodStocks.Select(kvp => "- " + kvp.Key + ": " + kvp.Value + "/" + Constants.MaxCountOfFoodStockForHouse);

            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();

            string result = string.Concat("It's a ", name,
                " with position ", currentPosition, "\r\nStocks inside :\r\n", string.Join(Environment.NewLine, linesS));

            result += "\r\n\r\n";
            result += "Current index of village: " + indexOfVillage;

            return result;
        }

    }
}
