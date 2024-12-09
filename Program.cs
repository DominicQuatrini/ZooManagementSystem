using System.Text;

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
    internal class MainMenuManager
    {
        private static void DisplayMainMenu() //Prints welcome messages, menu options, user income, and user balance
        {
            Console.WriteLine("== Welcome to the Zoo Management System! == \na) Animal Database \nb) Shop \nc) Collect money \nd) Save and Exit");
            Console.Write($"\nCurrent Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${Program.UserMoney}");
            Console.ResetColor();

            Console.Write($"\nIncome: +");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${Program.UserIncome}");
            Console.ResetColor();
            Console.WriteLine("/s");
        }
        private static string GetMainMenuChoice() //Gets user input for menu option
        {
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            return menuChoice;
        }
        private static void HandleMainMenuChoice(string menuChoice) //Calls the corresponding method based on the user's input
        {
            switch (menuChoice)
            {
                case "a":
                    Program.Database();
                    break;
                case "b":
                    if (Program.userAnimals.Count == 0) FreeShopManager.FreeShop();
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
        internal static void MainMenu()
        {
            Console.Clear();
            DisplayMainMenu();
            string menuChoice = GetMainMenuChoice();
            Console.Clear();
            HandleMainMenuChoice(menuChoice);
        }
    }
    internal class ShopManager
    {
        internal static Animal[] shopAnimals = new Animal[3];
        private static int priceA = 50;
        private static int factorB = 5;  private static int priceB = priceA * factorB; //Declare price equations to prevent redundancy
        private static int factorC = 10; private static int priceC = priceA * factorC;
        private static int factorM = 20; private static int priceM = priceA * factorM;
        internal static void InitializeShop() //Adds animals with varying exoticness (0, 1, and 2) to shopAnimals array. These are the first set of animals in the shop when the program is executed.
        {
            for (int i = 0; i < 3; i++) shopAnimals[i] = Animal.RandomAnimal(i);
        }
        private static void DisplayShopOffers(int i) //Prints each shop offer. Only one shop offer prints each time DisplayShopOffers(int i) is called
        {
            if (i >= shopAnimals.Length) return; //Stops the recursion if i is out of bounds for shopAnimals[i]

            Console.Write($"{(char)('a' + i)}) "); //Prints "a) ", "b) ", and "c) "
            shopAnimals[i].DisplayAnimal();

            if (shopAnimals[i].IsMythical == true) Console.WriteLine($", ${priceM}"); //This loop displays the corresponding price to the shop offer
            else if (i == 0) Console.WriteLine($", ${priceA}");
            else if (i == 1) Console.WriteLine($", ${priceB}");
            else Console.WriteLine($", ${priceC}");

            DisplayShopOffers(i + 1); //Recursion to loop printing the shop offers
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
                    PurchaseConfirmation(priceA, menuChoice);
                    break;
                case "b":
                    PurchaseConfirmation(priceB, menuChoice);
                    break;
                case "c":
                    if (shopAnimals[2].IsMythical == true) PurchaseConfirmation(priceM, menuChoice);
                    else PurchaseConfirmation(priceC, menuChoice);
                    break;
                case "d":
                    MainMenuManager.MainMenu();
                    break;
                default: break;
            }
        }
        private static void PurchaseConfirmation(int price, string userChoice) //Handles purchase interactions. Removes money from user's balance, adds purchased animal to user's animal. ALso restocks shop offers
        {
            Console.Clear();
            if (Program.UserMoney >= price)
            {
                int specialAnimalChance = Animal.rd.Next(10);
                int specialAnimalIndex = Animal.rd.Next(SpecialAnimal.specialAnimals.Count);
                int exoticness = Convert.ToChar(userChoice) - 'a';
                Animal purchasedAnimal = shopAnimals[exoticness];

                if (specialAnimalChance == 1 && userChoice == "c" && SpecialAnimal.specialAnimals.Count > 0) //Checking if specialAnimals.Count > 0 prevents crashing once all the mythical animals have been bought
                {
                    shopAnimals[exoticness] = SpecialAnimal.specialAnimals[specialAnimalIndex];
                    SpecialAnimal.specialAnimals.RemoveAt(specialAnimalIndex);
                }
                else
                    shopAnimals[exoticness] = Animal.RandomAnimal(exoticness);
                Program.UserMoney -= price;
                Program.userAnimals.Add(purchasedAnimal);
                Program.UpdateIncome();
                Console.WriteLine($"You purchased {purchasedAnimal.Name} the {purchasedAnimal.Species} for ${price}!");
            }
            else Console.WriteLine("Not enough money to purchase this animal.");
            Console.ReadLine();
            Console.Clear();
            Shop();
        }
        internal static void Shop()
        {
            Console.WriteLine($"== Welcome to the shop! ==");
            DisplayShopOffers(0);
            Console.WriteLine($"d) Back to main menu");
            string menuChoice = GetShopChoice();
            HandleShopChoice(menuChoice);
        }
    }
    internal class FreeShopManager
    {
        internal static Animal firstAnimal = new Animal("Rainier", "Rhino", 3, false);
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
                    Program.userAnimals.Add(firstAnimal);
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
            foreach (Animal animal in Program.userAnimals)
                saveInfo.AppendLine($"{animal.Name},{animal.Species},{animal.Income},{animal.IsMythical}");
            return saveInfo.ToString();
        }
        internal static void Save() //Saves the user's game data to save.txt
        {
            string saveInfo = SaveInfo();
            File.WriteAllText("save.txt", saveInfo);
            Console.Write("Saving");
            ExitAnimation();
            Console.WriteLine("\nData successfully saved!");
        }
    }
    internal class LoadSaveManager
    {
        private static void LoadMoney(string[] lines) //Loads the first line of save.txt as userMoney. If improperly formatted, userMoney will instead be set to 0
        {
            if (decimal.TryParse(lines[0], out decimal money))
                Program.UserMoney = money;
            else
            {
                Console.WriteLine("Invalid format in save file. Resetting user money to 0.\n");
                Program.UserMoney = 0;
            }
        }
        private static void LoadAnimals(string[] lines) //Reconstructs animals from animal data. If data is improperly formatted, an error message is displayed and reconstruction is skipped
        {
            for (int i = 1; i < lines.Length; i++)
            {
                string[] animalData = lines[i].Split(',');
                if (animalData.Length == 4 && int.TryParse(animalData[2], out int income))
                {
                    if (bool.TryParse(animalData[3], out bool isMythical))
                    {
                        if (isMythical)
                            Program.userAnimals.Add(new SpecialAnimal(animalData[0], animalData[1], income, true));
                        else
                            Program.userAnimals.Add(new Animal(animalData[0], animalData[1], income, false));
                    }
                    else Console.WriteLine($"Invalid animal isMythical data on line {i + 1}. Skipping.");
                }
                else Console.WriteLine($"Invalid animal income data on line {i + 1}. Skipping.");
            }
        }
        internal static void LoadSave() //Reads save.txt, reconstructs animals from animal data
        {
            if (File.Exists("save.txt"))
            {
                string[] lines = File.ReadAllLines("save.txt");
                if (lines.Length > 0)
                {
                    LoadMoney(lines);
                    LoadAnimals(lines);
                    Program.UpdateIncome();
                    Console.WriteLine("Save data loaded. Press enter to continue.");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Save file is empty or could not be found. Press enter to continue.");
            Program.UserMoney = 0;
            Console.ReadLine();
        }
    }
    internal class Program
    {
        internal static List<Animal> userAnimals = new List<Animal>();
        internal static decimal UserMoney { get; set; }
        internal static decimal UserIncome { get; set; }
        internal static DateTime lastIncomeTime;
        internal static void Database()
        {
            Console.WriteLine("== Welcome to the Database! ==");
            if (userAnimals.Count == 0) Console.WriteLine("You don't own any animals. Visit the shop."); //If the list of animals is empty, the user is prompted to visit the shop.
            else
            {
                for (int i = 0; i < userAnimals.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    if (userAnimals[i].IsMythical == true) userAnimals[i].DisplayAnimal();
                    else userAnimals[i].DisplayAnimal();
                    Console.WriteLine();
                }
            } //If the user has animals, it displays their name, species, and income
            Console.WriteLine("\nPress enter to return to the main menu.");
            Console.ReadLine();
            MainMenuManager.MainMenu(); //Once the user hits the enter key, they are sent back to the main menu 
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
        internal static void UpdateIncome() //Adds up the user's income per second, based off the sum of the user's animals' income stats
        {
            UserIncome = userAnimals.Sum(a => a.Income);
        }
        static void Main(string[] args)
        {
            LoadSaveManager.LoadSave();
            ShopManager.InitializeShop();
            lastIncomeTime = DateTime.Now;
            MainMenuManager.MainMenu();
        }
    }
}
/*
 * Abstraction/Interfaces
 * Combine GetShopChoice(), GetFreeShopChoice(), GetMainMenuChoice() methods into one, with a parameter numberOfOptions, to reduce reduncancy
 * Break up PurchaseConfirmation() into multiple methods, each with only a single objective
 * Include mythical data in save.txt, so that the user may only purchase 1 of each mythical. Currently, the mythical array resets to normal after the user reopens the program, meaning users can get more than 7 mythicals
 */