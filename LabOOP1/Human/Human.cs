using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1.Human
{
    public class Human 
    {
        (int, int) humanPosition;

        public Human((int, int) pos)
        {
            humanPosition = pos;
        }
    }
}
