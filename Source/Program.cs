using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiseaseSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.VariableInput();
            program.SimulationStart(program.MaxCycle);
            Console.WriteLine(" ");
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        public void VariableInput()
        {
            int number;
            double Double;

            //int populationsize;
            //double r0;
            //int initialInfected;

            Console.WriteLine("Please input the population size");
            bool flag1 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag1)
            {
                populationsize = number;
            }
            else
                Console.WriteLine("Not a valid integer");

            Console.WriteLine("Please input the Chance of Infection by contact (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag2 = Double.TryParse(Console.ReadLine(), out Double);
            if (flag2)
            {
                ChanceOfInfection = Double;
            }
            else
                Console.WriteLine("Not a valid value (don't input letters)");

            Console.WriteLine("Please input the Chance of Arrest of the infected individual (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag3 = Double.TryParse(Console.ReadLine(), out Double);
            if (flag3)
            {
                ChanceOfArrest = Double;
            }
            else
                Console.WriteLine("Not a valid value (don't input letters)");

            Console.WriteLine("Please input the Chance of Cure (in 100% scale, but don't input % sign e.g. 75.72381)");
            bool flag4 = Double.TryParse(Console.ReadLine(), out Double);
            if (flag4)
            {
                ChanceOfCure = Double;
            }
            else
                Console.WriteLine("Not a valid value (don't input letters)");

            Console.WriteLine("Please input the initial infected population");
            bool flag5 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag5)
            {
                initialInfected = number;
            }
            else
                Console.WriteLine("Not a valid integer");

            Console.WriteLine("Please input the maximum number of people contacted by a patient in one cycle");
            bool flag6 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag6)
            {
                PatientContact = number;
            }
            else
                Console.WriteLine("Not a valid integer");

            Console.WriteLine("Please input the Maximum Cycle for 1 simulation to run");
            bool flag7 = Int32.TryParse(Console.ReadLine(), out number);
            if (flag7)
            {
                MaxCycle = number;
            }
            else
                Console.WriteLine("Not a valid integer");

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
            Console.WriteLine("|=====|Initialization Finished|=====|");
            Console.ReadLine();

            currentContagious = currentInfected;
            Random rnd = new Random();

            for (int i = 1; i <= count; i++)
            {
                bool flag3 = currentInfected < populationsize;
                if (flag3)
                {
                    var CurrentContagious = currentContagious*(rnd.Next(PatientContact+1));
                    Console.WriteLine("CurrentContagious"+ CurrentContagious);
                    for (int I = 0; I < CurrentContagious; I++)
                    {
                        foreach (var Person in populaiton)
                        {
                            bool flag4 = Person.IsInfected;
                            if (!flag4)
                            {
                                if (rnd.Next(101) < ChanceOfInfection)
                                {
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
                            if (rnd.Next(101) < ChanceOfArrest && !flag6)
                            {
                                Person.IsArrested = true;
                                currentContagious--;
                            }
                            if (rnd.Next(101) < ChanceOfCure && flag6)
                            {
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                                currentContagious--;
                            }
                            if (rnd.Next(101) < ChanceOfCure && !flag6)
                            {
                                Person.IsInfected = false;
                                Person.IsArrested = false;
                                currentInfected--;
                            }
                        }
                    }

                    r0 = 1; //R0 formula not yet input
                    Console.WriteLine("Current Cases of Infection: " + currentInfected + ", Number of cycle: " + i + ". ");
                }
                else
                {
                    Console.WriteLine("The whole population is infected.");
                    break;
                }
            }
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

        public int populationsize;

        public double ChanceOfInfection;

        public double ChanceOfArrest;

        public double ChanceOfCure;

        public int initialInfected;

        public int PatientContact;

        public int currentInfected = 0;

        public int currentContagious; //If you are infected but you are arrested and quarantined, you are not contagious.

        public int MaxCycle;

        public double r0;

    }

    public class Person
    {
        public int Id { get; set; }
        public bool IsInfected { get; set; }
        public bool IsArrested { get; set; }
    }
}
