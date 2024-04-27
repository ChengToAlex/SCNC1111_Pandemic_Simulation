using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace DiseaseSimulation
{
    class main
    {
        Random rnd = new Random(); //Old randomizer, mscorlib built-in
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("|=====|Start|=====|");
            Console.WriteLine();

            Program program = new Program();
            //Seed = program.rnd.Next(0, 10000);
            Program.seed = (uint)DateTime.Now.GetHashCode();
            program.VariableInput();
            program.SimulationStart(program.MaxCycle);

            Console.WriteLine(" ");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
    class Program
    {
        

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
            Console.WriteLine("Current Contagious Population: " + currentContagious); //Console write the 0th Cycle(initial)
            Console.WriteLine("Current Cases of Infection: " + currentInfected + ", Number of cycle: " + 0 + ". "); //Console write the 0th Cycle
            Console.WriteLine("");

            List<Excel> excel = new List<Excel> { };
            excel.Add(new Excel { Cycle = 0, InfectedNumber = currentInfected, ContagiousNumber = currentContagious }); //Add 0th cycle data to excel list to be outputed
            bool poped = true;

            for (int i = 1; i <= count; i++)
            {
                //Console.WriteLine(Range(1, PatientContact)); // #####Debug#####
                bool flag3 = currentInfected < populationsize;
                if (flag3)
                {
                    var CurrentContagious = currentContagious*(Range(1 ,PatientContact));
                    CurrentContagious = (float)Math.Ceiling(CurrentContagious);

                    //var CurrentHealthy = populaiton.Count();
                    //Func<bool, bool> isInfected = IsInfected => !IsInfected;

                    var CurrentHealthy = populaiton.Count(Person => Person.IsInfected is false);
                    Console.WriteLine("Current Healthy Population: " + CurrentHealthy);
                    
                    //Console.WriteLine(CurrentContagious > CurrentHealthy); // #####Debug#####
                    
                    if (CurrentContagious > CurrentHealthy) { CurrentContagious = CurrentHealthy; }

                    Console.WriteLine("Total Number of Contact with patients:" + CurrentContagious);
                    for (int I = 1; I <= CurrentContagious; I++)
                    {
                        //Console.WriteLine("running CurrentContagious:" + I);
                        foreach (var Person in populaiton)
                        {
                            bool flag4 = Person.IsInfected;
                            if (!flag4)
                            {
                                //if (rnd.Next(101) < ChanceOfInfection) //!!!!!Outdated Random!!!!!
                                if (Chance(ChanceOfInfection))
                                {
                                    //Console.WriteLine(Chance(###Debug ChanceOfInfectionT/F:" + ChanceOfInfection)); // #####Debug#####
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
                        poped = false;
                        if (flag5)
                        {
                            //if (rnd.Next(101) < ChanceOfArrest && !flag6) //!!!!!Outdated Random!!!!!
                            if (Chance(ChanceOfArrest) && !flag6)
                            {
                                //Console.WriteLine(###Debug ChanceOfArrestT/F:" + Chance(ChanceOfArrest)); // #####Debug#####
                                Person.IsArrested = true;
                                currentContagious--;
                                poped = true;
                                //Console.WriteLine("###Debug currentContagious(Arresting):" + currentContagious); // #####Debug#####
                            }
                            //if (rnd.Next(101) < ChanceOfCure && flag6) //!!!!!Outdated Random!!!!!
                            if (Chance(ChanceOfCure) && flag6)
                            {
                                //Console.WriteLine(###Debug ChanceOfCureT/F:" + Chance(ChanceOfCure)); // #####Debug#####
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                                poped = true;
                                //Console.WriteLine("###Debug currentInfected(Arrested):" + currentInfected); // #####Debug#####
                            }
                            //if (rnd.Next(101) < ChanceOfCure && !flag6) //!!!!!Outdated Random!!!!!
                            if (Chance(ChanceOfCure) && !flag6 && !poped)
                            {
                                //Console.WriteLine("###Debug ChanceOfCureT/F:" + Chance(ChanceOfCure)); // #####Debug#####
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                                currentContagious--;
                                //Console.WriteLine("###Debug currentInfected(Not Arrested):" + currentInfected); // #####Debug#####
                                //Console.WriteLine("###Debug currentContagious(Not Arrested):" + currentContagious); // #####Debug#####
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The whole population is infected.");
                    break;
                }
                Console.WriteLine("Current Contagious Population: " + currentContagious);
                r0 = 1; //R0 formula not yet input
                Console.WriteLine("Current Cases of Infection: " + currentInfected + ", Number of cycle: " + i + ". ");
                Console.WriteLine("");
                excel.Add (new Excel {Cycle= i, InfectedNumber= currentInfected , ContagiousNumber= currentContagious });
            }
            Export<Excel>(excel, @"Output\output.xlsx", "Out");
        }

        public bool Export<T>(List<T> list, string file, string sheetname)
        {
            bool Exported = false;
            using (IXLWorkbook workbook = new XLWorkbook())
            {

                workbook.AddWorksheet(sheetname).FirstCell().InsertTable<T>(list, false);

                workbook.SaveAs(file);
                Exported = true;
            }

            return Exported;
        }

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

        public static float RandValue //input between 0 and 1 e.g. 75.95% = 0.7595
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

        public static uint seed;

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

    public class Excel
    {
        public int Cycle { get; set; }
        public int InfectedNumber { get; set; }
        public int ContagiousNumber { get; set; }
    }
}
