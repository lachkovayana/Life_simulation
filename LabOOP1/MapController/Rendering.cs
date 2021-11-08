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
        Image wolfImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneWolfCut.png");
        Image pigImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OnePigCut.png");
        Image rabbitImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneRabbitCut.png");
        Image bearImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneBearCut.png");
        Image horseImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneHorseCut.png");
        Image giraffeImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneGiraffeCut.png");
        Image foxImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneFoxCut.png");
        Image tigerImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneTigerCut.png");
        Image ratImg = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\OneRatCut.png");

        Image plantSeed = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plantSeed.png");
        Image plantPoisonousSeed = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plantPoisonousSeed.png");

        Image ePLantHealthySprout = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant1Sprout.png");
        Image ePLantHealthyFruitingSprout = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant3Sprout.png");
        Image ePLantPoisionousSprout = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant2Sprout.png");
        Image ePLantPoisonousFruitingSprout = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant4Sprout.png");

        Image ePLantHealthyGrown = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant1.png");
        Image ePLantHealthyFruitingGrown = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant3.png");
        Image ePLantPoisonousGrown = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant2.png");
        Image ePLantPoisonousFruitingGrown = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant4.png");

        Image ePLantHealthyDead = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant1Dead.png");
        Image ePLantHealthyFruitingDead = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant3Dead.png");
        Image ePLantPoisionousDead = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant2Dead.png");
        Image ePLantPoisonousFruitingDead = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\plant4Dead.png");


        Image iPlant = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\tree1.png");
        Image iPlantFruiting = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\tree2Grown.png");

        Image fruitHealthy = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\fruitHealthy.png");
        Image fruitPoisonous = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\fruitPoisonous.png");

        Image treeDead = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\tree2Dead.png");
        Image treeSeed = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\treeSeed.png");
        Image treeSprout = new Bitmap("C:\\Users\\Пользователь\\source\\repos\\OOP-LifeSimulation\\LabOOP1\\img\\treeSprout.png");

        public static FoodForOmnivorous[,] AllMapObjects = new FoodForOmnivorous[Form1.s_cols, Form1.s_rows];

        public void DrawFirstGeneration(MapObject mapObject, int x, int y)
        {
            //Form1.s_graphics.Clear(Color.Gainsboro);


            switch (mapObject)
            {
                case MapObject.rabbit:
                    //FillColorRectangle(x, y, (Brushes.Goldenrod));
                    //DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    Draw(rabbitImg, x, y);
                    break;
                case MapObject.horse:
                    //FillColorRectangle(x, y, (Brushes.Goldenrod));
                    //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    Draw(horseImg, x, y);
                    break;
                case MapObject.giraffe:
                    //FillColorRectangle(x, y, (Brushes.Goldenrod));
                    //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    Draw(giraffeImg, x, y);
                    break;
                case MapObject.tiger:
                    //FillColorRectangle(x, y, (Brushes.Purple));
                    //DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    Draw(tigerImg, x, y);

                    break;
                case MapObject.wolf:
                    //FillColorRectangle(x, y, (Brushes.Purple));
                    //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    Draw(wolfImg, x, y);
                    break;
                case MapObject.fox:
                    //FillColorRectangle(x, y, (Brushes.Purple));
                    //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    Draw(foxImg, x, y);
                    break;
                case MapObject.bear:
                    //FillColorRectangle(x, y, (Brushes.Pink));
                    //DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                    Draw(bearImg, x, y);
                    break;
                case MapObject.pig:
                    //FillColorRectangle(x, y, (Brushes.Pink));
                    //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                    Draw(pigImg, x, y);

                    break;
                case MapObject.rat:
                    //FillColorRectangle(x, y, (Brushes.Pink));
                    //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                    Draw(ratImg, x, y);
                    break;


                case MapObject.ediblePlantHealthy:
                    //FillColorEllipse(x, y, (Brushes.Lime));
                    Draw(plantSeed, x, y);
                    break;

                case MapObject.ediblePlantPoisonous:
                    //FillColorEllipse(x, y, (Brushes.Beige));
                    Draw(plantPoisonousSeed, x, y);
                    break;

                case MapObject.inediblePlant:
                    //FillColorEllipse(x, y, (Brushes.Tomato));
                    Draw(treeSeed, x, y);
                    break;

            }
            Form1.s_pictureBox.Refresh();


        }
        public void UpdateField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            AllMapObjects = new FoodForOmnivorous[Form1.s_cols, Form1.s_rows];
            Form1.s_graphics.Clear(Color.Gainsboro);

            foreach (Plant plant in listOfAllPlants)
            {
                int x = plant.GetPosition().Item1;
                int y = plant.GetPosition().Item2;

                AllMapObjects[x, y] = plant;

                switch (plant)
                {
                    case EdiblePlant plant1:
                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                if (plant1.IsHealthy())
                                    Draw(plantSeed, x, y);
                                //FillColorEllipse(x, y, Brushes.Lime);
                                else
                                    Draw(plantPoisonousSeed, x, y);
                                //FillColorEllipse(x, y, Brushes.Beige);
                                break;
                            case PlantStage.sprout:
                                //if (plant1.IsHealthy())
                                //    Draw(ePLantHealthySprout, x, y);
                                ////FillColorRectangle(x, y, Brushes.Lime);
                                //else
                                //    Draw(ePLantPoisionousSprout, x, y);
                                ////FillColorRectangle(x, y, Brushes.Beige);
                                //break;
                                if (plant1.IsHealthy())
                                {
                                    if (plant1.IsFruiting())
                                        Draw(ePLantHealthyFruitingSprout, x, y);
                                    else
                                        Draw(ePLantHealthySprout, x, y);
                                }
                                else
                                {
                                    if (plant1.IsFruiting())
                                        Draw(ePLantPoisionousSprout, x, y);
                                    else
                                        Draw(ePLantPoisonousFruitingSprout, x, y);
                                }
                                break;
                            case PlantStage.grown:
                                if (plant1.IsHealthy())
                                {
                                    if (plant1.IsFruiting())
                                    {
                                        Draw(ePLantHealthyFruitingGrown, x, y);
                                        //FillColorRectangle(x, y, Brushes.Green);
                                    }
                                    else
                                    {
                                        Draw(ePLantHealthyGrown, x, y);
                                        //FillColorRectangle(x, y, Brushes.SeaGreen);
                                    }
                                }
                                else
                                {
                                    if (plant1.IsFruiting())
                                        //FillColorRectangle(x, y, Brushes.White);
                                        Draw(ePLantPoisonousFruitingGrown, x, y);
                                    else
                                        Draw(ePLantPoisonousGrown, x, y);
                                    //FillColorRectangle(x, y, Brushes.LightGoldenrodYellow);
                                }

                                break;
                            case PlantStage.dead:
                                if (plant1.IsHealthy())
                                    Draw(ePLantHealthyDead, x, y);
                                //FillColorRectangle(x, y, Brushes.DarkOliveGreen);
                                else
                                    //FillColorRectangle(x, y, Brushes.Wheat);
                                    Draw(ePLantPoisionousDead, x, y);

                                break;
                        }
                        break;

                    case InediblePlant:

                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                //FillColorEllipse(x, y, Brushes.Tomato);
                                Draw(treeSeed, x, y);
                                break;
                            case PlantStage.sprout:
                                //FillColorRectangle(x, y, Brushes.Tomato);
                                Draw(treeSprout, x, y);
                                break;
                            case PlantStage.grown:
                                if (plant.IsFruiting())
                                    Draw(iPlantFruiting, x, y);
                                //FillColorRectangle(x, y, Brushes.Red);
                                else
                                    Draw(iPlant, x, y);
                                //FillColorRectangle(x, y, Brushes.Crimson);
                                break;
                            case PlantStage.dead:
                                //FillColorRectangle(x, y, Brushes.Maroon);
                                Draw(treeDead, x, y);
                                break;
                        }
                        break;
                }

            }
            foreach (Fruit fruit in listOfFruits)
            {
                int x = fruit.GetPosition().Item1;
                int y = fruit.GetPosition().Item2;
                AllMapObjects[x, y] = fruit;

                if (fruit.IsHealthy())
                {
                    //DrawColorEllipse(x, y, new Pen(Color.Chartreuse, 3));
                    Draw(fruitHealthy, x, y);
                }
                else
                {
                    //DrawColorEllipse(x, y, new Pen(Color.BurlyWood, 3));
                    Draw(fruitPoisonous, x, y);
                }


            }
            foreach (Animal animal in listOfAnimals)
            {
                int x = animal.GetPosition().Item1;
                int y = animal.GetPosition().Item2;

                AllMapObjects[x, y] = animal;

                switch (animal)
                {
                    case Rabbit:
                        //FillColorRectangle(x, y, (Brushes.Goldenrod));
                        //DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        Draw(rabbitImg, x, y);
                        break;
                    case Horse:
                        //FillColorRectangle(x, y, (Brushes.Goldenrod));
                        //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        Draw(horseImg, x, y);
                        break;
                    case Giraffe:
                        //FillColorRectangle(x, y, (Brushes.Goldenrod));
                        //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        Draw(giraffeImg, x, y);
                        break;
                    case Tiger:
                        //FillColorRectangle(x, y, (Brushes.Purple));
                        //DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        Draw(tigerImg, x, y);
                        break;
                    case Wolf:
                        //FillColorRectangle(x, y, (Brushes.Purple));
                        //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        Draw(wolfImg, x, y);
                        break;
                    case Fox:
                        //FillColorRectangle(x, y, (Brushes.Purple));
                        //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        Draw(foxImg, x, y);

                        break;
                    case Bear:
                        //    FillColorRectangle(x, y, (Brushes.Pink));
                        //    DrawColorRectangle(x, y, new Pen(Color.Chocolate, 3));
                        Draw(bearImg, x, y);
                        break;
                    case Pig:
                        //FillColorRectangle(x, y, (Brushes.Pink));
                        //DrawColorRectangle(x, y, new Pen(Color.PowderBlue, 3));
                        Draw(pigImg, x, y);
                        break;
                    case Rat:
                        //FillColorRectangle(x, y, (Brushes.Pink));
                        //DrawColorRectangle(x, y, new Pen(Color.Firebrick, 3));
                        Draw(ratImg, x, y);
                        break;
                }

            }
            Form1.s_pictureBox.Refresh();
        }
        //void FillColorEllipse(int x, int y, Brush br)
        //{
        //    Form1.s_graphics.FillEllipse(br, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        //}

        //void FillColorRectangle(int x, int y, Brush br)
        //{
        //    Form1.s_graphics.FillRectangle(br, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        //}
        //void DrawColorRectangle(int x, int y, Pen pen)
        //{
        //    Form1.s_graphics.DrawRectangle(pen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        //}
        //void DrawColorEllipse(int x, int y, Pen pen)
        //{
        //    Form1.s_graphics.DrawEllipse(pen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
        //}
        void Draw(Image img, int x, int y)
        {
            Form1.s_graphics.DrawImage(img, x * Form1.s_resolution, y * Form1.s_resolution, new Rectangle(new Point(0, 0), new Size(35, 35)), GraphicsUnit.Pixel);
        }
    }
}
