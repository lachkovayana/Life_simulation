using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

/* 
Gold золотой квадрат - животное
Lime зеленый круг - здоровое съедобное растение в стадии семени
Beige бежевый круг - ядовитое съедобное растение в стадии семени
Tomato красный круг - несъедобное растение в стадии семени
--------------------------

*/

namespace LabOOP1
{
    public class Rendering
    {
        public void DrawFirstGeneration(MapObject mapObject, int x, int y)
        {

            switch (mapObject)
            {
                case MapObject.rabbit:
                    FillColorRectangle(x, y, (Brushes.Goldenrod));
                    DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    break;
                case MapObject.horse:
                    FillColorRectangle(x, y, (Brushes.Goldenrod));
                    DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    break;
                case MapObject.giraffe:
                    FillColorRectangle(x, y, (Brushes.Goldenrod));
                    DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    break;
                case MapObject.leopard:
                    FillColorRectangle(x, y, (Brushes.Purple));
                    DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    break;
                case MapObject.wolf:
                    FillColorRectangle(x, y, (Brushes.Purple));
                    DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    break;
                case MapObject.fox:
                    FillColorRectangle(x, y, (Brushes.Purple));
                    DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    break;
                case MapObject.bear:
                    FillColorRectangle(x, y, (Brushes.Pink));
                    DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    break;
                case MapObject.pig:
                    FillColorRectangle(x, y, (Brushes.Pink));
                    DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    break;
                case MapObject.rat:
                    FillColorRectangle(x, y, (Brushes.Pink));
                    DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    break;


                case MapObject.ediblePlantHealthy:
                    FillColorEllipse(x, y, (Brushes.Lime));
                    break;

                case MapObject.ediblePlantPoisonous:
                    FillColorEllipse(x, y, (Brushes.Beige));
                    break;

                case MapObject.inediblePlant:
                    FillColorEllipse(x, y, (Brushes.Tomato));
                    break;

            }
            Form1.s_pictureBox.Refresh();


        }
        public void UpgradeField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            Form1.s_graphics.Clear(Color.Black);

            foreach (Plant plant in listOfAllPlants)
            {
                int x = plant.GetPosition().Item1;
                int y = plant.GetPosition().Item2;
                switch (plant)
                {
                    case EdiblePlant plant1:
                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                if (plant1.IsHealthy())
                                    FillColorEllipse(x, y, Brushes.Lime);
                                else
                                    FillColorEllipse(x, y, Brushes.Beige);
                                break;
                            case PlantStage.sprout:
                                if (plant1.IsHealthy())
                                    FillColorRectangle(x, y, Brushes.Lime);
                                else
                                    FillColorRectangle(x, y, Brushes.Beige);
                                break;
                            case PlantStage.grown:
                                if (plant1.IsHealthy())
                                {
                                    if (plant1.IsFruiting())
                                    {
                                        FillColorRectangle(x, y, Brushes.Green);
                                    }
                                    else
                                    {
                                        FillColorRectangle(x, y, Brushes.SeaGreen);
                                    }
                                }
                                else if (plant1.IsFruiting())
                                    FillColorRectangle(x, y, Brushes.White);
                                else
                                    FillColorRectangle(x, y, Brushes.LightGoldenrodYellow);

                                break;
                            case PlantStage.dead:
                                if (plant1.IsHealthy())
                                    FillColorRectangle(x, y, Brushes.DarkOliveGreen);
                                else
                                    FillColorRectangle(x, y, Brushes.Wheat);
                                break;
                        }
                        break;

                    case InediblePlant:

                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                FillColorEllipse(x, y, Brushes.Tomato);
                                break;
                            case PlantStage.sprout:
                                FillColorRectangle(x, y, Brushes.Tomato);
                                break;
                            case PlantStage.grown:
                                if (plant.IsFruiting())
                                    FillColorRectangle(x, y, Brushes.Red);
                                else
                                    FillColorRectangle(x, y, Brushes.Crimson);
                                break;
                            case PlantStage.dead:
                                FillColorRectangle(x, y, Brushes.Maroon);
                                break;
                        }
                        break;
                }

            }
            foreach (Fruit fruit in listOfFruits)
            {
                int x = fruit.GetPosition().Item1;
                int y = fruit.GetPosition().Item2;
                if (fruit.IsHealthy())
                {
                    DrawColorEllipse(x, y, new Pen(Color.Chartreuse, 3));
                }
                else
                {
                    DrawColorEllipse(x, y, new Pen(Color.BurlyWood, 3));
                }


            }
            foreach (Animal animal in listOfAnimals)
            {
                int x = animal.GetPosition().Item1;
                int y = animal.GetPosition().Item2;

                switch (animal)
                {
                    case Rabbit:
                        FillColorRectangle(x, y, (Brushes.Goldenrod));
                        DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        break;
                    case Horse:
                        FillColorRectangle(x, y, (Brushes.Goldenrod));
                        DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        break;
                    case Giraffe:
                        FillColorRectangle(x, y, (Brushes.Goldenrod));
                        DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        break;
                    case Leopard:
                        FillColorRectangle(x, y, (Brushes.Purple));
                        DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        break;
                    case Wolf:
                        FillColorRectangle(x, y, (Brushes.Purple));
                        DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        break;
                    case Fox:
                        FillColorRectangle(x, y, (Brushes.Purple));
                        DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        break;
                    case Bear:
                        FillColorRectangle(x, y, (Brushes.Pink));
                        DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        break;
                    case Pig:
                        FillColorRectangle(x, y, (Brushes.Pink));
                        DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        break;
                    case Rat:
                        FillColorRectangle(x, y, (Brushes.Pink));
                        DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        break;
                }

            }
                Form1.s_pictureBox.Refresh();
        }
        void FillColorEllipse(int x, int y, Brush br)
        {
            Form1.s_graphics.FillEllipse(br, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        }

        void FillColorRectangle(int x, int y, Brush br)
        {
            Form1.s_graphics.FillRectangle(br, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        }
        void DrawColorRectangle(int x, int y, Pen pen)
        {
            Form1.s_graphics.DrawRectangle(pen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        }
        void DrawColorEllipse(int x, int y, Pen pen)
        {
            Form1.s_graphics.DrawEllipse(pen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        }

    }
}
