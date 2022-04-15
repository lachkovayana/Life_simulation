using System;

namespace LabOOP1
{
    public class Movement
    {
        // случайное движение 
        public (int, int) MoveToRCOrdinary((int, int) pos)
        {
            return GetClosestCell(pos);
        }

        //случайное движение, не отходя больше чем на 2 клетки от места рождения/последней еды
        public (int, int) MoveToRCNotGoingFar((int, int) pos, (int, int) basisCellPos, Animal an)
        {
            Random rnd = new();
            int x, y;
            do
            {
                x = pos.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols || Math.Abs(x - basisCellPos.Item1) > 2);

            do
            {
                y = pos.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows || Math.Abs(y - basisCellPos.Item2) > 2);

            return (x, y);
        }

        // случайное движение с вероятностью 50% к средней точке по всем существам вида
        public (int, int) MoveToRCWithProbability(Animal thisAnimal)
        {
            int x = 0;
            int y = 0;
            int count = 0;
            foreach (Animal f in MapObjectsControl.listOfAnimalsCopy)
            {
                if (f.GetType() == thisAnimal.GetType())
                {
                    x += f.GetPosition().Item1;
                    y += f.GetPosition().Item2;
                    count++;
                }
            }
            if (count == 0) return (0, 0);
            (int, int) averPoint = (x / count, y / count);

            Random random = new();
            int rand = random.Next(0, 10);

            (int, int) newPosition;
            if (rand < 5)
                newPosition = MoveToTargetFor8Cells(thisAnimal.GetPosition(), averPoint);
            else
                newPosition = MoveToRCOrdinary(thisAnimal.GetPosition());

            return newPosition;
        }

        // движение к еде в 8 сторон
        public (int, int) MoveToTargetFor8Cells((int, int) pos, (int, int) targetPos)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - targetPos.Item1;
            var disty = pos.Item2 - targetPos.Item2;

            if (distx < 0)
            {
                if (disty > 0)
                    newPosAn = MoveToDirection(pos, Direction.rightUp);
                else if (disty < 0)
                    newPosAn = MoveToDirection(pos, Direction.rightDown);
                else
                    newPosAn = MoveToDirection(pos, Direction.right);
            }
            else if (distx > 0)
            {
                if (disty > 0)
                    newPosAn = MoveToDirection(pos, Direction.leftUp);
                else if (disty < 0)
                    newPosAn = MoveToDirection(pos, Direction.leftDown);
                else
                    newPosAn = MoveToDirection(pos, Direction.left);
            }
            else
            {
                if (disty > 0)
                    newPosAn = MoveToDirection(pos, Direction.up); 
                else if (disty < 0)
                    newPosAn = MoveToDirection(pos, Direction.down); 
            }
            return newPosAn;
        }


        // движение к еде в 4 стороны с сохранением направления
        public (int, int) MoveToTargetFor4Cells((int, int) pos, (int, int) targetPos)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - targetPos.Item1;
            var disty = pos.Item2 - targetPos.Item2;

            if (distx > 0)
                newPosAn = MoveToDirection(pos, Direction.left);
            else if (distx < 0)
                newPosAn = MoveToDirection(pos, Direction.right);
            else
            {
                if (disty > 0)
                    newPosAn = MoveToDirection(pos, Direction.up);
                else if (disty < 0)
                    newPosAn = MoveToDirection(pos, Direction.down);
            }

            return newPosAn;
        }

        // Движение с шагом в 3 клетки
        public (int, int) MoveToTarget3CellsForward((int, int) pos, (int, int) targetPos)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - targetPos.Item1;
            var disty = pos.Item2 - targetPos.Item2;
            int x = pos.Item1;
            int y = pos.Item2;

            if (distx < 0)
            {
                if (disty > 0)
                    newPosAn = (x + Math.Min(3, Math.Abs(distx)), y - Math.Min(3, Math.Abs(disty)));
                else if (disty < 0)
                    newPosAn = (x + Math.Min(3, Math.Abs(distx)), y + Math.Min(3, Math.Abs(disty)));
                else
                    newPosAn = (x + Math.Min(3, Math.Abs(distx)), y);
            }
            else if (distx > 0)
            {
                if (disty > 0)
                    newPosAn = (x - Math.Min(3, Math.Abs(distx)), y - Math.Min(3, Math.Abs(disty)));
                else if (disty < 0)
                    newPosAn = (x - Math.Min(3, Math.Abs(distx)), y + Math.Min(3, Math.Abs(disty)));
                else
                    newPosAn = (x - Math.Min(3, Math.Abs(distx)), y);
            }
            else
            {
                if (disty > 0)
                    newPosAn = (x, y - Math.Min(3, Math.Abs(disty)));
                else if (disty < 0)
                    newPosAn = (x, y + Math.Min(3, Math.Abs(disty)));
            }


            return newPosAn;
        }



        //-------------------------------------------вспомогательные функции-------------------------------------------

        //поиск новой клетки в пределах поля
        public (int, int) GetClosestCell((int, int) position)
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = position.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = position.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);
            return (x, y);
        }

        // Расстояние L1
        public int CountDistL1((int, int) posAnimal, (int, int) posFood)
        {
            var tmpx = Math.Abs(posAnimal.Item1 - posFood.Item1);
            var tmpy = Math.Abs(posAnimal.Item2 - posFood.Item2);
            return tmpx + tmpy;
        }

        // Евклидово расстояние
        public double CountDistEuclid((int, int) posAnimal, (int, int) posFood)
        {
            var tmpx = Math.Abs(posAnimal.Item1 - posFood.Item1);
            var tmpy = Math.Abs(posAnimal.Item2 - posFood.Item2);
            return Math.Sqrt(Math.Pow(tmpx, 2) + Math.Pow(tmpy, 2));
        }

        private (int, int) MoveToDirection((int, int) pos, Direction direction)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            switch (direction)
            {
                case Direction.right:
                    return (x + 1, y);
                case Direction.left:
                    return (x - 1, y);
                case Direction.down:
                    return (x, y + 1);
                case Direction.up:
                    return (x, y - 1);
                case Direction.rightDown:
                    return (x + 1, y + 1);
                case Direction.rightUp:
                    return (x + 1, y - 1);
                case Direction.leftDown:
                    return (x - 1, y + 1);
                case Direction.leftUp:
                    return (x - 1, y - 1);
                default:
                    break;
            }
            return (x, y);
        }
    }
}
