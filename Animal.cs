using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class Animal
    {
        internal static Random rd = new Random();
        internal string Name { get; set; }
        internal string Species { get; set; }
        internal int Income { get; set; }
        internal bool IsMythical { get; set; }
        internal Animal() //Default constructor is required for class SpecialAnimal to inherit from class Animal
        {

        }
        internal Animal(string name, string species, int income, bool isMythical)
        {
            this.Name = name;
            this.Species = species;
            this.Income = income;
            this.IsMythical = isMythical;
        }
        internal virtual void DisplayAnimal()
        {
            Console.Write($"{Name} the {Species}, ${Income}/s");
        }
        private static string RandomName() //Generates a random name from a names.txt
        { //names are from https://www.ssa.gov/oact/babynames/decades/century.html
            if (File.Exists("names.txt"))
            {
                string[] lines = File.ReadAllLines("names.txt");
                int n = rd.Next(lines.Length);
                return lines[n];
            }
            else return "placeholder";
        }
        private enum SpeciesOptions
        {
            Cobra,
            Eagle,
            Horse,
            Bull,
            Alligator,
            Penguin,
            Bear,
            Whale,
            Goat,
            Deer,
            Boar,
            Rabbit,
            Beaver,
            Otter,
            Tiger,
            Lion,
            Hippo,
            Seal,
            Dolphin,
            Orca,
            Gazelle,
            Shark,
            Tarantula,
            Mosquito,
            Beetle,
            Scorpion,
            Meerkat,
            Fox,
            Jellyfish,
            Octopus,
            Leopard,
            Wolf,
            Monkey,
            Turtle
        }
        private static string RandomSpecies()
        {
            string[] species = Enum.GetNames(typeof(SpeciesOptions));
            int n = rd.Next(species.Length);
            return species[n];
        } //Generates a random species from enum SpeciesOptions
        private static int RandomIncome(int exoticness) //Generates a random income. int exoticness dictates the animal's minimum and maximum possible income
        {
            if (exoticness == 0)
                return rd.Next(1, 6);
            else if (exoticness == 1)
                return rd.Next(6, 11);
            else
                return rd.Next(11, 16);
        }
        internal static Animal RandomAnimal(int exoticness) //Generates an animal object using the aforementioned randomized methods. 
        {
            string randomName = RandomName();
            string randomSpecies = RandomSpecies();
            int randomIncome = RandomIncome(exoticness);

            return new Animal(randomName, randomSpecies, randomIncome, false);
        }
    }
    internal class SpecialAnimal : Animal
    {
        internal static List<SpecialAnimal> specialAnimals = new List<SpecialAnimal>
        {
            new SpecialAnimal("Asterion", "Minotaur", 20, true),
            new SpecialAnimal("Twilight Sparkle", "Unicorn", 20, true),
            new SpecialAnimal("Typhon", "Hydra", 20, true),
            new SpecialAnimal("Karkinos", "Leviathan", 20, true),
            new SpecialAnimal("Zenyatta", "Phoenix", 20, true),
            new SpecialAnimal("Spyro", "Dragon", 20, true),
            new SpecialAnimal("Mordecai", "Griffin", 20, true)
        }; //List of mythical animals with preset fields
        internal SpecialAnimal(string name, string species, int income, bool isMythical)
        {
            this.Name = name;
            this.Species = species;
            this.Income = income;
            this.IsMythical = isMythical;
        }
        internal override void DisplayAnimal() //Overrides Animal.DisplayAnimal() to allow different formatting and font color
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"The Great {Species}");
            Console.ResetColor();
            Console.Write(", ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{Name}");
            Console.ResetColor();
            Console.Write($", ${Income}/s");
        }
    }
}
