using System;

namespace LabOOP1
{
    public class Movement
    {

        public (int, int) MoveToRandomCell((int, int) pos)
        {
            Random rnd = new();

            int x, y;
            do
            {
                x = pos.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = pos.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);

            return (x, y);
        }

        public (int, int) MoveToTarget((int, int) pos, FoodForOmnivores target)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - target.GetPosition().Item1;
            var disty = pos.Item2 - target.GetPosition().Item2;

            if (distx < 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.up);
                }
                else
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                }
            }
            else if (distx > 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.up); ;
                }
                else
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                }
            }
            else
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.down); ;

                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.up); ;

                }
                else
                {
                    
                }
            }
            return newPosAn;
        }

        public (int, int) MoveToDirection((int, int) pos, Direction direction)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            switch (direction)
            {
                case Direction.right:
                    return (x + 1, y);
                case Direction.left:
                    return (x - 1, y);
                case Direction.up:
                    return (x, y + 1);
                case Direction.down:
                    return (x, y - 1);
                default:
                    break;
            }
            return (x, y);
        }
    }
}
