namespace LabOOP1
{
    public abstract class FoodForOmnivorous
    {
        protected (int, int) currentPosition;
        public FoodForOmnivorous((int, int) pos)
        {
            currentPosition = pos;
        }
        internal (int, int) GetPosition() => currentPosition;
        public string GetInfoAndLight()
        {
            Rendering.LightChoosen(currentPosition.Item1, currentPosition.Item2);
            return GetInfo();
        }
        protected virtual string GetInfo() { return ""; }
    }
}