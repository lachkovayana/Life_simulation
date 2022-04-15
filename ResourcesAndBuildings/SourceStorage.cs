//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LabOOP1
//{
//    public interface IStorage<TItem, TKey>
//    {
//        ICollection<TItem> GetItems();
//        TItem GetItem(TKey key);
//        void AddItem(TItem newItem);
//        void DeleteItem(TKey key);
//    }

//    public class SourceStorage : IStorage<Source, (int, int)>
//    {
//        private ICollection<Source> _sources;
//        private int _countOfRes = 5;
//        public ICollection<Source> GetItems()
//        {
//            if (_countOfRes > 0)
//            {
//                _countOfRes--;
//                return _sources;
//            }
//            else return default;
//        }

//        public Source GetItem((int, int) pos)
//        {
//            return _sources.FirstOrDefault(x => x.Position == pos);
//        }

//        public void AddItem(Source s)
//        {
//            _sources.Add(s);
//        }

//        public void DeleteItem((int, int) pos)
//        {
//            var sourceToRemove = _sources.FirstOrDefault(x => x.Position == pos);
//            if (sourceToRemove != null)
//            {
//                _sources.Remove(sourceToRemove);
//            }
//        }
//    }
//}
