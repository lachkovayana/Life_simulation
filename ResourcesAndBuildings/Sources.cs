using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class Source
    {
        public (int, int) Position { get; set; }
        public int Count { get; set; }
    }
    //public class Source<T> where T : new()
    //{
    //    public int CountOfRes = 5;

    //    public (int, int) p;
    //    public T GetItem()
    //    {
    //        if (CountOfRes > 0)
    //        {
    //            CountOfRes--;
    //            return new T();
    //        }

    //        else return default; //null
    //    }
    //}
    public class GoldSource : Source
    {
        public GoldSource((int, int) pos) { }
    }
    public class IronSource : Source
    {
        public IronSource((int, int) pos) { }
    }
    public class StoneSource : Source
    {
        public StoneSource((int, int) pos) { }
    }

    public class WoodSource : Source
    {
        public WoodSource((int, int) pos) { }
    }

}
