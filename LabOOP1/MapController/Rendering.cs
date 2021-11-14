using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;


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

        public static FoodForOmnivorous[,] FieldOfAllMapObjects = new FoodForOmnivorous[Form1.s_cols, Form1.s_rows];

        Color seasonColor = Color.Gainsboro;

        public static (int, int) coorLight;

        public void UpdateField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<Animal> listOfHumans)
        {
            UpdateSeasonColor();
            ClearField();
            UpdatePlants(listOfAllPlants);
            UpdateFruits(listOfFruits);
            UpdateHumans(listOfHumans);
            UpdateAnimals(listOfAnimals);

            Form1.s_pictureBox.Refresh();

        }
        void UpdateSeasonColor()
        {
            seasonColor = (MapObjectsControl.s_currentSeason == Season.summer) ? Color.DarkSeaGreen : Color.Gainsboro;
        }
        void ClearField()
        {
            FieldOfAllMapObjects = new FoodForOmnivorous[Form1.s_cols, Form1.s_rows];
            Form1.s_graphics.Clear(seasonColor);
        }
        private void UpdateHumans(List<Animal> listOfHumans)
        {
            foreach (Human h in listOfHumans)
            {
                int x = h.GetPosition().Item1;
                int y = h.GetPosition().Item2;
                FieldOfAllMapObjects[x, y] = h;
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

        void UpdatePlants(List<Plant> listOfPlants)
        {
            foreach (Plant plant in listOfPlants)
            {
                int x = plant.GetPosition().Item1;
                int y = plant.GetPosition().Item2;

                FieldOfAllMapObjects[x, y] = plant;

                switch (plant)
                {
                    case EdiblePlant plant1:
                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                if (plant1.IsHealthy())
                                    Draw(plantSeed, x, y);
                                else
                                    Draw(plantPoisonousSeed, x, y);
                                break;
                            case PlantStage.sprout:
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
                                if (plant1.IsHealthy())
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


        void UpdateFruits(List<Fruit> listOfFruits)
        {
            foreach (Fruit fruit in listOfFruits)
            {
                int x = fruit.GetPosition().Item1;
                int y = fruit.GetPosition().Item2;
                FieldOfAllMapObjects[x, y] = fruit;

                if (fruit.IsHealthy())
                {
                    Draw(fruitHealthy, x, y);
                }
                else
                {
                    Draw(fruitPoisonous, x, y);
                }
            }
        }

        void UpdateAnimals(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                int x = animal.GetPosition().Item1;
                int y = animal.GetPosition().Item2;

                FieldOfAllMapObjects[x, y] = animal;
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
        }
    }
}
