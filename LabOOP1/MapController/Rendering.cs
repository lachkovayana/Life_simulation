using System.Drawing;
using System.Collections.Generic;
using System;

namespace LabOOP1
{
    public class Rendering
    {
        Image wolfImg = Image.FromFile("../../../img/OneWolfCut.png");
        Image pigImg = Image.FromFile("../../../img/OnePigCut.png");
        Image rabbitImg = Image.FromFile("../../../img/OneRabbitCut.png");
        Image bearImg = Image.FromFile("../../../img/OneBearCut.png");
        Image horseImg = Image.FromFile("../../../img/OneHorseCut.png");
        Image sheepImg = Image.FromFile("../../../img/OneSheep.png");
        Image foxImg = Image.FromFile("../../../img/OneFoxCut.png");
        Image tigerImg = Image.FromFile("../../../img/OneTigerCut.png");
        Image ratImg = Image.FromFile("../../../img/OneRatCut.png");

        Image plantSeed = Image.FromFile("../../../img/seed.png");
        Image plantPoisonousSeed = Image.FromFile("../../../img/plantPoisonousSeed.png");

        Image ePLantHealthySprout = Image.FromFile("../../../img/plant1Sprout.png");
        Image ePLantHealthyFruitingSprout = Image.FromFile("../../../img/plant3Sprout.png");
        Image ePLantPoisionousSprout = Image.FromFile("../../../img/plantPoisonousSprout.png");
        Image ePLantPoisonousFruitingSprout = Image.FromFile("../../../img/plant2Sprout.png");

        Image ePLantHealthyGrown = Image.FromFile("../../../img/plant1.png");
        Image ePLantHealthyFruitingGrown = Image.FromFile("../../../img/plant3.png");
        Image ePLantPoisonousGrown = Image.FromFile("../../../img/cactus2.png");
        Image ePLantPoisonousFruitingGrown = Image.FromFile("../../../img/cactus1.png");

        Image ePLantHealthyDead = Image.FromFile("../../../img/plant1Dead.png");
        Image ePLantHealthyFruitingDead = Image.FromFile("../../../img/plant3Dead.png");
        Image ePLantPoisionousDead = Image.FromFile("../../../img/plant2Dead.png");
        Image ePLantPoisonousFruitingDead = Image.FromFile("../../../img/plant4Dead.png");


        Image iPlant = Image.FromFile("../../../img/tree1.png");
        Image iPlantFruiting = Image.FromFile("../../../img/tree2Grown.png");

        Image fruitHealthy = Image.FromFile("../../../img/fruitHealthy.png");
        Image fruitPoisonous = Image.FromFile("../../../img/fruitPoisonous.png");

        Image treeDead = Image.FromFile("../../../img/tree2Dead.png");
        Image treeSeed = Image.FromFile("../../../img/treeSeed.png");
        Image treeSprout = Image.FromFile("../../../img/treeSprout.png");

        Image man = Image.FromFile("../../../img/human.png");
        Image woman = Image.FromFile("../../../img/primitiveWoman.png");

        Image meat = Image.FromFile("../../../img/meat.png");

        Image goldSourse = Image.FromFile("../../../img/gold.png");
        Image ironSourse = Image.FromFile("../../../img/iron.png");
        Image stoneSourse = Image.FromFile("../../../img/stone.png");
        Image woodSourse = Image.FromFile("../../../img/wood.png");

        Image house = Image.FromFile("../../../img/house1.png");
        Image barn = Image.FromFile("../../../img/building1.png");


        Color seasonColor = Color.Gainsboro;

        public static (int, int) coorLight;

        public void DrawField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<Animal> listOfHumans)
        {
            DrawSeasonColor();
            ClearField();
            DrawPlants(listOfAllPlants);
            DrawFruits(listOfFruits);
            DrawHumans(listOfHumans);
            DrawAnimals(listOfAnimals);
            DrawHouses();
            Form1.s_pictureBox.Refresh();

        }

        private void DrawHouses()
        {
            foreach (Building b in MapObjectsControl.ListOfBuildings)
            {
                int x = b.GetPosition().Item1;
                int y = b.GetPosition().Item2;
                switch (b)
                {
                    case House:
                        Draw(house, x, y);
                        break;
                    case Barn:
                        Draw(barn, x, y);
                        break;
                }
                

                MapObjectsControl.FieldOfAllMapObjects[x, y].Add(b);
            }
        }

        //private void DrawSources(MyList<Source> listOfSources)
        //{
        //    foreach (Source in listOfSources)
        //    {
        //        switch (Source)
        //        {
        //            case PlantStage.seed:
        //                Draw(treeSeed, x, y);
        //                break;
        //            case PlantStage.sprout:
        //                Draw(treeSprout, x, y);
        //                break;
        //            case PlantStage.grown:
        //                if (plant.IsFruiting())
        //                    Draw(iPlantFruiting, x, y);
        //                else
        //                    Draw(iPlant, x, y);
        //                break;
        //            case PlantStage.dead:
        //                Draw(treeDead, x, y);
        //                break;
        //        }
        //    }
        //}

        void DrawSeasonColor()
        {
            seasonColor = (MapObjectsControl.s_currentSeason == Season.summer) ? Color.DarkSeaGreen : Color.Gainsboro;
        }
        void ClearField()
        {
           //MapObjectsControl.FieldOfAllMapObjects = new List<MapObject>[Form1.s_cols, Form1.s_rows];
            Form1.s_graphics.Clear(seasonColor);
        }
        private void DrawHumans(List<Animal> listOfHumans)
        {
            foreach (Human h in listOfHumans)
            {
                int x = h.GetPosition().Item1;
                int y = h.GetPosition().Item2;
                MapObjectsControl.FieldOfAllMapObjects[x, y].Add(h);
                switch (h.gender)
                {
                    case Gender.female:
                        Draw(woman, x, y);
                        break;
                    case Gender.male:
                        Draw(man, x, y);
                        break;

                }
            }
        }

        void DrawPlants(List<Plant> listOfPlants)
        {
            foreach (Plant plant in listOfPlants)
            {
                int x = plant.GetPosition().Item1;
                int y = plant.GetPosition().Item2;

                MapObjectsControl.FieldOfAllMapObjects[x, y].Add(plant);

                switch (plant)
                {
                    case EdiblePlant plant1:
                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                if (plant1.IsHealthy)
                                    Draw(plantSeed, x, y);
                                else
                                    Draw(plantPoisonousSeed, x, y);
                                break;
                            case PlantStage.sprout:
                                if (plant1.IsHealthy)
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
                                if (plant1.IsHealthy)
                                {
                                    if (plant1.IsFruiting())
                                    {
                                        Draw(ePLantHealthyFruitingGrown, x, y);
                                    }
                                    else
                                    {
                                        Draw(ePLantHealthyGrown, x, y);
                                    }
                                }
                                else
                                {
                                    if (plant1.IsFruiting())
                                        Draw(ePLantPoisonousFruitingGrown, x, y);
                                    else
                                        Draw(ePLantPoisonousGrown, x, y);
                                }

                                break;
                            case PlantStage.dead:
                                if (plant1.IsHealthy)
                                    Draw(ePLantHealthyDead, x, y);
                                else
                                    Draw(ePLantPoisionousDead, x, y);

                                break;
                        }
                        break;

                    case InediblePlant:

                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                Draw(treeSeed, x, y);
                                break;
                            case PlantStage.sprout:
                                Draw(treeSprout, x, y);
                                break;
                            case PlantStage.grown:
                                if (plant.IsFruiting())
                                    Draw(iPlantFruiting, x, y);
                                else
                                    Draw(iPlant, x, y);
                                break;
                            case PlantStage.dead:
                                Draw(treeDead, x, y);
                                break;
                        }
                        break;
                }
            }
        }


        void DrawFruits(List<Fruit> listOfFruits)
        {
            foreach (Fruit fruit in listOfFruits)
            {
                int x = fruit.GetPosition().Item1;
                int y = fruit.GetPosition().Item2;
                MapObjectsControl.FieldOfAllMapObjects[x, y].Add(fruit);

                if (fruit.IsHealthy)
                {
                    Draw(fruitHealthy, x, y);
                }
                else
                {
                    Draw(fruitPoisonous, x, y);
                }
            }
        }

        void DrawAnimals(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                int x = animal.GetPosition().Item1;
                int y = animal.GetPosition().Item2;

                MapObjectsControl.FieldOfAllMapObjects[x, y].Add(animal);
                if (animal.IsDead)
                    Draw(meat, x, y);
                else
                {
                    switch (animal)
                    {
                        case Rabbit:
                            Draw(rabbitImg, x, y);
                            break;
                        case Horse:
                            Draw(horseImg, x, y);
                            break;
                        case Sheep:
                            Draw(sheepImg, x, y);
                            break;
                        case Tiger:
                            Draw(tigerImg, x, y);
                            break;
                        case Wolf:
                            Draw(wolfImg, x, y);
                            break;
                        case Fox:
                            Draw(foxImg, x, y);
                            break;
                        case Bear:
                            Draw(bearImg, x, y);
                            break;
                        case Pig:
                            Draw(pigImg, x, y);
                            break;
                        case Rat:
                            Draw(ratImg, x, y);
                            break;
                    }
                }
            }
        }
        public static void LightChoosen(int x, int y)
        {
            Form1.s_graphics.DrawRectangle(new Pen(Color.PowderBlue, 3), x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
            Form1.s_pictureBox.Refresh();
        }
        void Draw(Image img, int x, int y)
        {
            Form1.s_graphics.DrawImage(img, x * Form1.s_resolution, y * Form1.s_resolution, new Rectangle(new Point(0, 0), new Size(35, 35)), GraphicsUnit.Pixel);
            //Form1.s_graphics.DrawImage(img, x * Form1.s_resolution, y * Form1.s_resolution);
        }
    }
}
