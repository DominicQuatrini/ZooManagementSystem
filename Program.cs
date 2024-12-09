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
    internal class MainMenuManager
    {
        internal static void MainMenu()
        {
            Console.Clear();
            DisplayMainMenu();
            string menuChoice = GetMenuChoice();
            Console.Clear();
            HandleMenuChoice(menuChoice);
        }
        private static void DisplayMainMenu() //Prints welcome messages, menu options, user income, and user balance
        {
            Console.WriteLine("== Welcome to the Zoo Management System! == \na) Animal Database \nb) Shop \nc) Collect money \nd) Save and Exit");
            Console.Write($"\nCurrent Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${Program.UserMoney}");
            Console.ResetColor();

            Console.Write($"\nIncome: +");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${Program.UserIncome}"); //Prints current user money and income
            Console.ResetColor();
            Console.WriteLine("/s");
        }
        private static string GetMenuChoice() //Gets user input for menu option
        {
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            return menuChoice;
        }
        private static void HandleMenuChoice(string menuChoice) //Calls the corresponding method based on the user's input
        {
            switch (menuChoice)
            {
                case "a":
                    Program.Database();
                    break;
                case "b":
                    if (Program.animals.Count == 0) FreeShopManager.FreeShop();
                    else ShopManager.Shop();
                    break;
                case "c":
                    Program.CollectMoney();
                    break;
                case "d":
                    SaveManager.Save();
                    break;
                default:
                    break;
            }
        }
    }
    internal class ShopManager
    {
        internal static Animal[] shopAnimals = new Animal[3];
        private static void DisplayShop()
        {
            Console.WriteLine($"== Welcome to the shop! ==");
            int basePrice = 0;
            for (int i = 0; i < shopAnimals.Length; i++)
            {
                Console.Write($"{(char)('a' + i)}) ");
                shopAnimals[i].DisplayAnimal();
                Console.WriteLine($", ${basePrice + 50 * (i + 1)}");
                basePrice += 50;
            }
            Console.WriteLine($"d) Back to main menu");
        }
        private static string GetShopChoice()
        {
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            return menuChoice;
        }
        private static void HandleShopChoice(string menuChoice)
        {
            switch (menuChoice)
            {
                case "a":
                    Program.PurchaseConfirmation(50, menuChoice);
                    break;
                case "b":
                    Program.PurchaseConfirmation(150, menuChoice);
                    break;
                case "c":
                    Program.PurchaseConfirmation(250, menuChoice);
                    break;
                case "d":
                    MainMenuManager.MainMenu();
                    break;
                default: break;
            }
        }
        internal static void Shop()
        {
            DisplayShop();
            string menuChoice = GetShopChoice();
            HandleShopChoice(menuChoice);
        }
    }
    internal class FreeShopManager
    {
        static Animal firstAnimal = new Animal("Rainier", "Rhino", 3, false);
        private static string GetFreeShopChoice()
        {
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            return menuChoice;
        }
        private static void HandleFreeShopChoice(string menuChoice)
        {
            switch (menuChoice)
            {
                case "a":
                    Program.animals.Add(firstAnimal);
                    Program.UpdateIncome();
                    Console.WriteLine($"You purchased {firstAnimal.Name} the {firstAnimal.Species}! \nPress enter to go back to the main menu.");
                    Console.ReadLine();
                    MainMenuManager.MainMenu();
                    break;
                case "b":
                    MainMenuManager.MainMenu();
                    break;
                default: break;
            }
        }
        internal static void FreeShop() //Opens a shop with one free animal offer
        {

            Console.WriteLine($"== Welcome to the shop! ==" +
                $"\na) {firstAnimal.Name} the {firstAnimal.Species}, FREE" +
                $"\nb) Back to main menu");

            string menuChoice = GetFreeShopChoice();
            HandleFreeShopChoice(menuChoice);
        }
    }
    internal class SaveManager
    {
        internal static void Save() //Saves the user's game data to save.txt
        {
            string saveInfo = SaveInfo();
            File.WriteAllText("save.txt", saveInfo);
            Console.Write("Saving");
            ExitAnimation();
            Console.WriteLine("\nData successfully saved!");
        }
        private static void ExitAnimation() //Prints "Saving" -> "Saving ." -> "Saving . ." ->  "Saving . . ." with intervals between each update
        {
            DateTime temp = DateTime.Now;
            double delay = 0.75; //Change this value to change the interval between each update
            int periodCount = 0;
            while (periodCount < 3)
            {
                if ((DateTime.Now - temp).TotalSeconds >= delay)
                {
                    Console.Write(" .");
                    temp = DateTime.Now;
                    periodCount++;
                }
            }
        }
        private static string SaveInfo() //Returns userMoney and user's animal data as one string
        {
            StringBuilder saveInfo = new StringBuilder(); //Must use StringBuilder because strings are immutable after initialized
            saveInfo.AppendLine(Program.UserMoney.ToString());
            foreach (Animal animal in Program.animals)
                saveInfo.AppendLine($"{animal.Name},{animal.Species},{animal.Income},{animal.IsMythical}");
            return saveInfo.ToString();
        }
    }
    internal class Program
    {
        internal static List<Animal> animals = new List<Animal>();
        private static DateTime lastIncomeTime;
        internal static decimal UserMoney { get; set; }
        internal static decimal UserIncome { get; set; }
        internal static void Database()
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
            MainMenuManager.MainMenu(); //Once the user hits the enter key, they are sent back to the main menu 
        }
        internal static void PurchaseConfirmation(int price, string userChoice) //Handles purchase interactions. Removes money from user's balance, adds purchased animal to user's animal. ALso restocks shop offers
        {
            Console.Clear();
            if (UserMoney >= price)
            {
                int specialAnimalChance = Animal.rd.Next(2);
                int specialAnimalIndex = Animal.rd.Next(SpecialAnimal.specialAnimals.Count);
                Animal purchasedAnimal;
                if (userChoice == "a")
                {
                    purchasedAnimal = ShopManager.shopAnimals[0];
                    ShopManager.shopAnimals[0] = Animal.RandomAnimal(1); //Replaces the purchased animal with a new random animal with matching exoticness
                }
                else if (userChoice == "b")
                {
                    purchasedAnimal = ShopManager.shopAnimals[1];
                    ShopManager.shopAnimals[1] = Animal.RandomAnimal(2);
                }
                else
                {
                    purchasedAnimal = ShopManager.shopAnimals[2];
                    if(specialAnimalChance == 1)
                    { //If the user purchases all of the special animals, the game bugs out and crashes.
                        ShopManager.shopAnimals[2] = SpecialAnimal.specialAnimals[specialAnimalIndex];
                        SpecialAnimal.specialAnimals.RemoveAt(specialAnimalIndex);
                    }
                    else ShopManager.shopAnimals[2] = Animal.RandomAnimal(3);
                }
                UserMoney -= price;
                animals.Add(purchasedAnimal); //Adds the purchased animal to the list of the user's animals
                UpdateIncome();
                Console.WriteLine($"You purchased {purchasedAnimal.Name} the {purchasedAnimal.Species} for ${price}!");
            }
            else Console.WriteLine("Not enough money to purchase this animal.");
            Console.ReadLine();
            Console.Clear();
            ShopManager.Shop();
        }
        internal static void UpdateIncome() //Adds up the user's income per second, based off the sum of the user's animals' income stats
        {
            UserIncome = animals.Sum(a => a.Income);
        }
        
        private static void LoadSave() //Reads save.txt, reconstructs animals from animal data
        {
            if (File.Exists("save.txt"))
            {
                string[] lines = File.ReadAllLines("save.txt");
                if (lines.Length > 0)
                {
                    if (decimal.TryParse(lines[0], out decimal money)) //Loads the first line of save.txt as userMoney. If improperly formatted, userMoney will instead be set to 0
                        UserMoney = money;
                    else
                    {
                        Console.WriteLine("Invalid format in save file. Resetting user money to 0.\n");
                        UserMoney = 0;
                    } 
                    for (int i = 1; i < lines.Length; i++) //Reconstructs animals from animal data. If data is improperly formatted, an error message is displayed and reconstruction is skipped
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
                    UpdateIncome();
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
        internal static void CollectMoney() //Calculates how much money the user's animals have earned, then adds it to the user's balance
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
            MainMenuManager.MainMenu();
        }
        private static void InitializeShop()
        {
            for (int i = 0; i < 3; i++)
            {
                ShopManager.shopAnimals[i] = Animal.RandomAnimal(i + 1);
                //Adds animals with varying exoticness to an array. These are the first set of animals in the shop when the program is executed.
            }
        }
        static void Main(string[] args)
        {
            LoadSave();
            InitializeShop();
            lastIncomeTime = DateTime.Now;
            MainMenuManager.MainMenu();
        }
    }
}
/*
 * Recursion
 * Abstraction/Interfaces
 */