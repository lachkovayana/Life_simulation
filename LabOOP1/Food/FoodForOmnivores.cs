namespace LabOOP1
{
    public abstract class FoodForOmnivorous : MapObject
    {
        //protected abstract int NutritionalUnit { get;}
        protected abstract string GetInfo();

        protected (int, int) currentPosition;
        public FoodForOmnivorous((int, int) pos)
        {
            currentPosition = pos;
        }
        internal (int, int) GetPosition() => currentPosition;
        public override string GetInfoAndLight()
        {
            Rendering.LightChoosen(currentPosition.Item1, currentPosition.Item2);
            return GetInfo();
        }
        

}
}