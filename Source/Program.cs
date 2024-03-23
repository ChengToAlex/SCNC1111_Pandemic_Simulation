using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace DiseaseSimulation
{
    class Program
    {
        Random rnd = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("|=====|Start|=====|");
            Console.WriteLine();

            Program program = new Program();
            Seed = program.rnd.Next(0, 10000);
            program.VariableInput();
            program.SimulationStart(program.MaxCycle);

            Console.WriteLine(" ");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static int Seed
        {
            set
            {
                seed = (uint)RandValue;// for randomizer
                iterations = 0U;// for randomizer
            }
        }

        public void VariableInput()
        {
            int number;
            float Float;

            repeat1:
            Console.WriteLine("Please input the population size");
            bool flag1 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag1)
            {
                populationsize = number;
            }
            else
            {
                Console.WriteLine("Not a valid integer");
                goto repeat1;
            }

            repeat2:
            Console.WriteLine("Please input the Chance of Infection by contact (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag2 = float.TryParse(Console.ReadLine(), out Float);
            bool flag2_1 = Float <= 100;
            if (flag2 && flag2_1)
            {
                ChanceOfInfection = Float;
            }
            else
            {
                Console.WriteLine("Not a valid value (don't input letters)");
                goto repeat2;
            }

            repeat3:
            Console.WriteLine("Please input the Chance of Arrest of the infected individual (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag3 = float.TryParse(Console.ReadLine(), out Float);
            if (flag3 && flag2_1)
            {
                ChanceOfArrest = Float;
            }
            else
            {
                Console.WriteLine("Not a valid value (don't input letters)");
                goto repeat3;
            }

            repeat4:
            Console.WriteLine("Please input the Chance of Cure (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag4 = float.TryParse(Console.ReadLine(), out Float);
            if (flag4 && flag2_1)
            {
                ChanceOfCure = Float;
            }
            else
            {
                Console.WriteLine("Not a valid value (don't input letters)");
                goto repeat4;
            }

            repeat5:
            Console.WriteLine("Please input the initial infected population");
            bool flag5 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag5)
            {
                initialInfected = number;
            }
            else
            {
                Console.WriteLine("Not a valid integer");
                goto repeat5;
            }

            repeat6:
            Console.WriteLine("Please input the maximum number of people contacted by a patient in one cycle");
            bool flag6 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag6)
            {
                PatientContact = number;
            }
            else
            {
                Console.WriteLine("Not a valid integer");
                goto repeat6;
            }

            repeat7:
            Console.WriteLine("Please input the Maximum Cycle for 1 simulation to run");
            bool flag7 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag7)
            {
                MaxCycle = number;
            }
            else
            {
                Console.WriteLine("Not a valid integer");
                goto repeat7;
            }
            ChanceOfCure = ChanceOfCure / 100;
            ChanceOfArrest = ChanceOfArrest / 100;
            ChanceOfInfection = ChanceOfInfection / 100;

            Console.WriteLine("");
            Console.WriteLine("|=====|Input Finished|=====|");
            Console.WriteLine("");
        }

        public void SimulationStart(int count)
        {
            //currentInfected = initialInfected;
            var populaiton = GetPopulation(populationsize); //generate a list of people of the population size
            bool NotInitial = currentInfected != initialInfected;
            if (initialInfected > populationsize)
            {
                Console.WriteLine("!!!Error!!! initial infected population is bigger than population size");
                return;
            }

            //Initializing, set IsInfected to true for initialInfected number of people
            foreach (var Person in populaiton) 
            {
                bool flag1 = initialInfected < populationsize;
                bool flag2 = currentInfected < initialInfected;
                if (flag1 && flag2)
                {
                    Console.WriteLine("ID: " + Person.Id + " is now infected");
                    Person.IsInfected = true;
                    currentInfected++;
                }
                else
                    break;
            }
            Console.WriteLine("");
            Console.WriteLine("|=====|Initialization Finished|=====|");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

            currentContagious = currentInfected;

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine("Current Contagious Population: " + currentContagious);
                r0 = 1; //R0 formula not yet input
                Console.WriteLine("Current Cases of Infection: " + currentInfected + ", Number of cycle: " + i + ". ");

                //Console.WriteLine(Range(1, PatientContact));
                bool flag3 = currentInfected < populationsize;
                if (flag3)
                {
                    var CurrentContagious = currentContagious*(Range(1 ,PatientContact));
                    for (int I = 1; I <= CurrentContagious; I++)
                    {
                        //Console.WriteLine("running CurrentContagious:" + I);
                        foreach (var Person in populaiton)
                        {
                            bool flag4 = Person.IsInfected;
                            if (!flag4)
                            {
                                //if (rnd.Next(101) < ChanceOfInfection)
                                if (Chance(ChanceOfInfection))
                                {
                                    //Console.WriteLine(Chance(ChanceOfInfection));
                                    Person.IsInfected = true;
                                    currentInfected++;
                                    currentContagious++;
                                }
                                break;
                            }
                        }
                    }

                    foreach (var Person in populaiton)
                    {
                        bool flag5 = Person.IsInfected;
                        bool flag6 = Person.IsArrested;
                        if (flag5)
                        {
                            //if (rnd.Next(101) < ChanceOfArrest && !flag6)
                            if (Chance(ChanceOfArrest) && !flag6)
                            {
                                //Console.WriteLine(Chance(ChanceOfArrest));
                                Person.IsArrested = true;
                                currentContagious--;
                            }
                            //if (rnd.Next(101) < ChanceOfCure && flag6)
                            if (Chance(ChanceOfCure) && flag6)
                            {
                                //Console.WriteLine(Chance(ChanceOfCure));
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                            }
                            //if (rnd.Next(101) < ChanceOfCure && !flag6)
                            if (Chance(ChanceOfCure) && !flag6)
                            {
                                //Console.WriteLine(Chance(ChanceOfCure));
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                                currentContagious--;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The whole population is infected.");
                    break;
                }
            }
        }

        /*
        class XML
        {
            XLWorkbook workbook = new XLWorkbook();
            DataTable dt = new DataTable() { TableName = "New Worksheet" };
            DataSet ds = new DataSet();
            var columns = new[] { "column1", "column2", "column3" };
            var rows = new object[][]
            {
                new object[] {"1", 2, false },
                new object[] { "test", 10000, 19.9 }
            };

            //Add columns
            dt.Columns.AddRange(columns.Select(c => new DataColumn(c)).ToArray());

            //Add rows
            foreach (var row in rows)
            {
                //dt.Rows.Add(row);
            }

            //Convert datatable to dataset and add it to the workbook as worksheet
            ds.Tables.Add(dt);
            workbook.Worksheets.Add(ds);

            //save
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savePath = Path.Combine(desktopPath, "test.xlsx");
            workbook.SaveAs(savePath, false);
        }
        */

        static IEnumerable<Person> GetPopulation(int count)
        {
            List<Person> population = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                population.Add(new Person() {Id = i, IsInfected = false, IsArrested = false});
            }
            return population;
        }

        //Randomize from other coder
        public static int GetInt(uint seed, uint input)
        {
            uint num = input * 3432918353U;
            num = (num << 15 | num >> 17);
            num *= 461845907U;
            uint num2 = seed ^ num;
            num2 = (num2 << 13 | num2 >> 19);
            num2 = num2 * 5U + 3864292196U;
            num2 ^= 2834544218U;
            num2 ^= num2 >> 16;
            num2 *= 2246822507U;
            num2 ^= num2 >> 13;
            num2 *= 3266489909U;
            return (int)(num2 ^ num2 >> 16);
        }
        public static float Range(float min, float max)
        {
            if (max <= min)
            {
                return min;
            }
            return RandValue * (max - min) + min;
        }
        public static float RandValue
        {
            get
            {
                return (float)(((double)GetInt(seed, iterations++) - -2147483648.0) / 4294967295.0);
            }
        }

        public static bool Chance(float chance)
        {
            return chance > 0f && (chance >= 1f || RandValue < chance);
        }

        private static uint seed;

        private static uint iterations = 0U;
        //Randomize from other coder

        public int populationsize;

        public float ChanceOfInfection;

        public float ChanceOfArrest;

        public float ChanceOfCure;

        public int initialInfected;

        public int PatientContact;

        public int currentInfected = 0;

        public int currentContagious; //If you are infected but you are arrested and quarantined, you are not contagious.

        public int MaxCycle;

        public double r0; //for reverse calculation of R0 only

    }

    public class Person
    {
        public int Id { get; set; }
        public bool IsInfected { get; set; }
        public bool IsArrested { get; set; }
    }
}
