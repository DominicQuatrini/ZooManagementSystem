using System.Text;

namespace ZooManagementSystem
{
    internal class Animal
    {
        public static Random rd = new Random();
        public string Name { get; set; }
        public string Species { get; set; }
        public int Income { get; set; }
        public bool IsMythical { get; set; }
        public Animal()
        {

        }
        public Animal(string name, string species, int income, bool isMythical)
        {
            this.Name = name;
            this.Species = species;
            this.Income = income;
            this.IsMythical = isMythical;
        }
        public virtual void DisplayAnimal()
        {
            Console.Write($"{Name} the {Species}, ${Income}/s");
        }
        private static string RandomName()
        { //names are from https://www.ssa.gov/oact/babynames/decades/century.html
            if (File.Exists("names.txt"))
            {
                string[] lines = File.ReadAllLines("names.txt");
                int n = rd.Next(lines.Length);
                return lines[n];
            } //Returns a random line from names.txt
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
        } //Returns a random species from the Species enum
        public static Animal RandomAnimal(int exoticness)
        {
            string rName = RandomName();
            string rSpecies = RandomSpecies();
            int rIncome = rd.Next(1, 7) * exoticness; //Gives a random value from 1 to 6            
            return new Animal(rName, rSpecies, rIncome, false);
        }
    }
    internal class SpecialAnimal : Animal
    {
        public static List<SpecialAnimal> specialAnimals = new List<SpecialAnimal>
        {
            new SpecialAnimal("Asterion", "Minotaur", 20, true),
            new SpecialAnimal("Twilight Sparkle", "Unicorn", 20, true),
            new SpecialAnimal("Typhon", "Hydra", 20, true),
            new SpecialAnimal("Karkinos", "Leviathan", 20, true),
            new SpecialAnimal("Zenyatta", "Phoenix", 20, true),
            new SpecialAnimal("Spyro", "Dragon", 20, true),
            new SpecialAnimal("Mordecai", "Griffin", 20, true)
        };
        public SpecialAnimal(string name, string species, int income, bool isMythical)
        {
            this.Name = name;
            this.Species = species;
            this.Income = income;
            this.IsMythical = isMythical;
        }
        public override void DisplayAnimal()
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
    internal class Program
    {
        private static List<Animal> animals = new List<Animal>();
        private static Animal[] shopAnimals = new Animal[3];
        private static DateTime lastIncomeTime;
        public static decimal UserMoney { get; set; }
        public static decimal UserIncome { get; set; }
        private static void MainMenu()
        {
            Console.WriteLine("== Welcome to the Zoo Management System! == \na) Animal Database \nb) Shop \nc) Collect money \nd) Save and Exit");
            Console.Write($"\nCurrent Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${UserMoney}");
            Console.ResetColor();

            Console.Write($"\nIncome: +");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${UserIncome}"); //Prints current user money and income
            Console.ResetColor();
            Console.WriteLine("/s");

            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            } //Prompts user for input. If they enter anything other than A, a, B, b, C, c, then it will return an error message and prompt them to enter another option.
            Console.Clear();
            switch (menuChoice)
            {
                case "a":
                    Database();
                    break;
                case "b":
                    if(animals.Count == 0) FreeShop();
                    else Shop(); //If the user doesn't have any animals, a different shop menu will be opened
                    break;
                case "c":
                    CollectMoney();
                    break;
                case "d":
                    SaveExit();
                    break;
                default:
                    break;
            } //Calls the function that corresponds to the user's input
        }
        private static void Database()
        {
            Console.WriteLine("== Welcome to the Database! ==");
            if (animals.Count == 0) Console.WriteLine("You don't own any animals. Visit the shop."); //If the list of animals is empty, the user is prompted to visit the shop.
            else
            {
                for (int i = 0; i < animals.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    if (animals[i].IsMythical == true) animals[i].DisplayAnimal();
                    else animals[i].DisplayAnimal();
                    Console.WriteLine();
                }
            } //If the user has animals, it displays their name, species, and income
            Console.WriteLine("\nPress enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu(); //Once the user hits the enter key, they are sent back to the main menu 
        }
        private static void InitializeShop()
        {
            for (int i = 0; i < 3; i++)
            {
                shopAnimals[i] = Animal.RandomAnimal(i + 1);
                //Adds animals with varying exoticness to an array. These are the first set of animals in the shop when the program is executed.
            }
        }
        private static void Shop()
        {
            Console.WriteLine($"== Welcome to the shop! ==");
            int basePrice = 0;
            for (int i = 0; i < shopAnimals.Length; i++)
            {
                Console.Write($"{(char)('a' + i)}) ");
                shopAnimals[i].DisplayAnimal();
                Console.WriteLine($", ${basePrice + 50*(i + 1)}");
                basePrice += 50;
            }
            Console.WriteLine($"d) Back to main menu");
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            } //Once again, requires the user to enter A, a, B, b, C, c, D, d to continue. Else, they are prompted to input a different option
            Console.Clear();
            switch (menuChoice)
            {
                case "a":
                    PurchaseConfirmation(50, menuChoice);
                    break;
                case "b":
                    PurchaseConfirmation(150, menuChoice);
                    break;
                case "c":
                    PurchaseConfirmation(250, menuChoice);
                    break;
                case "d":
                    MainMenu();
                    break;
                default: break;
            }
        }
        private static void PurchaseConfirmation(int price, string userChoice)
        {
            if (UserMoney >= price)
            {
                int specialAnimalChance = Animal.rd.Next(2);
                int specialAnimalIndex = Animal.rd.Next(SpecialAnimal.specialAnimals.Count);
                Animal purchasedAnimal;
                if (userChoice == "a")
                {
                    purchasedAnimal = shopAnimals[0];
                    shopAnimals[0] = Animal.RandomAnimal(1); //Replaces the purchased animal with a new random animal with matching exoticness
                }
                else if (userChoice == "b")
                {
                    purchasedAnimal = shopAnimals[1];
                    shopAnimals[1] = Animal.RandomAnimal(2);
                }
                else
                {
                    purchasedAnimal = shopAnimals[2];
                    if(specialAnimalChance == 1)
                    { //If the user purchases all of the special animals, the game bugs out and crashes.
                        shopAnimals[2] = SpecialAnimal.specialAnimals[specialAnimalIndex];
                        SpecialAnimal.specialAnimals.RemoveAt(specialAnimalIndex);
                    }
                    else shopAnimals[2] = Animal.RandomAnimal(3);
                }
                UserMoney -= price;
                animals.Add(purchasedAnimal); //Adds the purchased animal to the list of the user's animals
                UpdateIncome();
                Console.WriteLine($"You purchased {purchasedAnimal.Name} the {purchasedAnimal.Species} for ${price}!");
            }
            else Console.WriteLine("Not enough money to purchase this animal.");
            Console.ReadLine();
            Console.Clear();
            Shop();
        }
        private static void FreeShop()
        {
            Animal firstAnimal = new Animal("Rainier", "Rhino", 3, false);
            Console.WriteLine($"== Welcome to the shop! ==" +
                $"\na) {firstAnimal.Name} the {firstAnimal.Species}, FREE" +
                $"\nb) Back to main menu");
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            switch(menuChoice)
            {
                case "a":
                    animals.Add(firstAnimal);
                    UpdateIncome();
                    Console.WriteLine($"You purchased {firstAnimal.Name} the {firstAnimal.Species}! \nPress enter to go back to the main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    MainMenu();
                    break;
                case "b":
                    Console.Clear();
                    MainMenu();
                    break;
                default: break;
            }
        }
        private static void UpdateIncome()
        { //Adds up all of the income from the user's animals, to be received per second. When called, updates the user's income to include newly purchased animals
            UserIncome = animals.Sum(a => a.Income);
        }
        private static void SaveExit()
        {
            string saveInfo = SaveInfo();
            File.WriteAllText("save.txt", saveInfo); //Writes the returned value of saveInfo to a .txt file
            DateTime temp = DateTime.Now;
            int periodCount = 0;
            Console.Write("Saving");
            while (periodCount < 3)
            {
                if ((DateTime.Now - temp).TotalSeconds >= 0.5)
                { //Prints "Saving" -> "Saving ." -> "Saving . ." ->  "Saving . . ." with half-second intervals between updates
                    Console.Write(" .");
                    temp = DateTime.Now;
                    periodCount++;
                }
            }
            Console.WriteLine("\nData successfully saved!");
        }
        private static string SaveInfo()
        {
            StringBuilder saveInfo = new StringBuilder();
            saveInfo.AppendLine(UserMoney.ToString());
            foreach (Animal animal in animals)
            {
                saveInfo.AppendLine($"{animal.Name},{animal.Species},{animal.Income},{animal.IsMythical}");
            } //The first line of saveInfo is the user's money, every line after that is animal data
            return saveInfo.ToString();
        }
        private static void LoadSave()
        {
            if (File.Exists("save.txt"))
            {
                string[] lines = File.ReadAllLines("save.txt"); //Converts save.txt to an array, with each index being a different line in the .txt
                if (lines.Length > 0)
                {
                    if (decimal.TryParse(lines[0], out decimal money)) //Loads the first line of save.txt as userMoney. If improperly formatted, userMoney will instead be set to 0
                    {
                        UserMoney = money;
                    }
                    else
                    {
                        Console.WriteLine("Invalid format in save file. Resetting user money to 0.\n");
                        UserMoney = 0;
                    } 
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] animalData = lines[i].Split(',');
                        if (animalData.Length == 4 && int.TryParse(animalData[2], out int income))
                        {
                            if (animalData[3] == "True")
                                animals.Add(new SpecialAnimal(animalData[0], animalData[1], income, true));
                            else
                                animals.Add(new Animal(animalData[0], animalData[1], income, false));
                        }
                        else Console.WriteLine($"Invalid animal data on line {i + 1}. Skipping.");
                    }
                    UpdateIncome(); //Updates user income based on loaded animals
                    Console.WriteLine("Save data loaded.\n");
                }
                else
                {
                    Console.WriteLine("Save file is empty. Starting fresh.\n");
                    UserMoney = 0;
                }
            }
            else
            {
                Console.WriteLine("No save file found. Starting fresh.\n");
                UserMoney = 0;
            }
        }
        private static void CollectMoney()
        {
            int secondsElapsed = (int)Math.Round((DateTime.Now - lastIncomeTime).TotalSeconds);
            UserMoney += UserIncome * secondsElapsed;
            Console.Write($"You earned ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${UserIncome * secondsElapsed} ");
            Console.ResetColor();
            Console.WriteLine($"from your animals!");
            lastIncomeTime = DateTime.Now;

            Console.WriteLine("\nPress enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }
        static void Main(string[] args)
        {
            LoadSave();
            InitializeShop();
            lastIncomeTime = DateTime.Now;
            MainMenu();
        }
    }
}
/*
 * Recursion
 * Abstraction/Interfaces
 */