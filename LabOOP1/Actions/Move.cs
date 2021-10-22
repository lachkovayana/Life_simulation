using System;


namespace LabOOP1
{

    public class Move
    {
        private (int, int) _position;
        //private int _cols;
        //private int _rows;

        public Move((int, int) position)
        {
            _position = position;
        }


        //public (int, int) FindNewCell()
        //{
        //    Random rnd = new Random();

        //    int x, y;
        //    do
        //    {
        //        x = _position.Item1 + rnd.Next(-1, 2);
        //    }
        //    while (x < 0 || x >= _cols);

        //    do
        //    {
        //        y = _position.Item2 + rnd.Next(-1, 2);
        //    }
        //    while (y < 0 || y >= _rows);
        //    return (x, y);
        //}
    }
}

