using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace ZooManagementSystem
{
    class Animal
    {
        static Random rd = new Random();
        public string name; public string species; public int income;
        public Animal(string name, string species, int income)
        {
            this.name = name;
            this.species = species;
            this.income = income;
        }
        public void DisplayAnimal()
        {
            Console.WriteLine($"{name} the {species}, $/s: {income}");
        }
        static string RandomName()
        {
            if (File.Exists("names.txt"))
            {
                string[] lines = File.ReadAllLines("names.txt");
                int n = rd.Next(lines.Length);
                return lines[n];
            } //Returns a random line from names.txt
            else return "placeholder";
        }
        enum Species
        {
            Elephant,
            Rhino,
            Hippo,
            Giraffe,
            Cheetah,
            Bear,
            Eagle,
            Whale
        }
        static string RandomSpecies()
        {
            string[] species = Enum.GetNames(typeof(Species));
            int n = rd.Next(species.Length);
            return species[n];
        } //Returns a random species from the Species enum
        public static Animal RandomAnimal(int exoticness)
        {
            string rName = RandomName();
            string rSpecies = RandomSpecies();
            int rIncome = rd.Next(1, 7) * exoticness; //Gives a random value from 1 to 6            
            return new Animal(rName, rSpecies, rIncome);
        }
    }
    class Program
    {
        static List<Animal> animals = new List<Animal>();
        //creates a list of all of the user's animals within the class scope. Purchased animals will be added to this list
        static Stack<Animal> shopAnimals = new Stack<Animal>();
        static decimal userMoney = 0;
        static decimal userIncome = 0;
        static HashSet<Animal> uniqueAnimals = new HashSet<Animal>();
        static void MainMenu()
        {
            Console.WriteLine("Welcome to the Zoo Management System! Please choose an option. \na) Animal Database \nb) Shop \nc) Save and Exit");
            Console.WriteLine($"Current Money: ${userMoney:F2} \nIncome per second: ${userIncome:F2}"); //prints current user money and income

            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c")
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
                    if(animals.Count == 0)
                    {
                        FreeShop();
                    }
                    else Shop();
                    break;
                case "c":
                    SaveExit();
                    break;
                default:
                    break;
            } //Calls the function that corresponds to the user's input
        }
        static void Database()
        {
            if (animals.Count == 0)
                Console.WriteLine("You don't own any animals. Visit the shop."); //If the list of animals is empty, the user is prompted to visit the shop.
            else
            {
                for (int i = 0; i < animals.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    animals[i].DisplayAnimal();
                }
            } //If the user has animals, it displays their name, species, and income
            Console.WriteLine("Press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu(); //Once the user hits the enter key, they are sent back to the main menu
        }
        static void InitializeShop()
        {
            for (int i = 0; i < 3; i++)
            {
                shopAnimals.Push(Animal.RandomAnimal(3 - i));
                //Puts the animal with exoticness 3 at the bottom of the stack, exoticness 2 in the middle, and exoticness 1 at the top
            }
        }
        static void Shop()
        {
            Animal[] currentAnimals = shopAnimals.ToArray();
            //Converts the stack shopAnimals into an array --> [0] = exoticness 3, [1] = exoticness 2, [2] = exoticness 1
            Console.WriteLine($"Welcome to the shop!" + 
                              $"\na) {currentAnimals[2].name} the {currentAnimals[2].species}, $50" +
                              $"\nb) {currentAnimals[1].name} the {currentAnimals[1].species}, $150" +
                              $"\nc) {currentAnimals[0].name} the {currentAnimals[0].species}, $250" +
                              $"\nd) Back to main menu");
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
                    PurchaseConfirmation(50);
                    break;
                case "b":
                    PurchaseConfirmation(150);
                    break;
                case "c":
                    PurchaseConfirmation(250);
                    break;
                case "d":
                    MainMenu();
                    break;
                default: break;
            }
        }
        static void FreeShop()
        {
            Animal firstAnimal = new Animal("Rainier", "Rhino", 3);
            Console.WriteLine($"Welcome to the shop!" +
                $"\na) {firstAnimal.name} the {firstAnimal.species}, FREE" +
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
                    Console.WriteLine($"Purchased {firstAnimal.name} the {firstAnimal.species}. \nPress enter to go back to the main menu.");
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
        static void PurchaseConfirmation(int price)
        {
            if (userMoney >= price)
            {
                Animal purchasedAnimal = shopAnimals.Pop(); //Removes the purchased animal from the shop selection
                animals.Add(purchasedAnimal); //Adds the purchased animal to the list of the user's animals
                userMoney -= price;
                UpdateIncome();
                Console.WriteLine($"You purchased {purchasedAnimal.name} the {purchasedAnimal.species} for ${price}!");
                ShopRestock();
            }
            else
            {
                Console.WriteLine("Not enough money to purchase this animal.");
            }
            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }
        static void ShopRestock()
        { //Once an animal is purchased, a new, randomized one takes its place in the shop.
            while (shopAnimals.Count < 3)
            {
                shopAnimals.Push(Animal.RandomAnimal(3 - shopAnimals.Count));
            } //The new animal's exoticness is based off of how many animals are currently in the shop
        }
        static void UpdateIncome()
        { //Adds up all of the income from the user's animals, to be received per second. When called, updates the user's income to include newly purchased animals
            userIncome = animals.Sum(a => a.income);
        }
        static void EarnedIncome()
        {
            userMoney += userIncome;
            Console.WriteLine($"You earned ${userIncome:F2} from your animals!");
        }
        static void SaveExit()
        {
            string saveInfo = SaveInfo();
            File.WriteAllText("save.txt", saveInfo); //Writes the returned value of saveInfo to a .txt file
            Console.WriteLine("Saving . . .");
            Console.WriteLine("Data successfully saved!");
        }
        static string SaveInfo()
        {
            uniqueAnimals.Clear();
            uniqueAnimals.UnionWith(animals); //Adds all user animals to uniqueAnimals hashset

            StringBuilder saveInfo = new StringBuilder();
            saveInfo.AppendLine(userMoney.ToString());
            foreach (Animal animal in uniqueAnimals)
            {
                saveInfo.AppendLine($"{animal.name},{animal.species},{animal.income}");
            } //The first line of saveInfo is the user's money, every line after that is animal data
            return saveInfo.ToString();
        }
        static void LoadSave()
        {
            if (File.Exists("save.txt"))
            {
                string[] lines = File.ReadAllLines("save.txt"); //Converts save.txt to an array, with each index being a different line in the .txt
                if (lines.Length > 0)
                {
                    if (decimal.TryParse(lines[0], out decimal money)) //Loads the first line of save.txt as userMoney. If improperly formatted, userMoney will instead be set to 0
                    {
                        userMoney = money;
                    }
                    else
                    {
                        Console.WriteLine("Invalid format in save file. Resetting user money to 0.");
                        userMoney = 0;
                    } 
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] animalData = lines[i].Split(',');
                        if (animalData.Length == 3 && int.TryParse(animalData[2], out int income))
                        { //If the line from save.txt contains the properly formatted animal data, the animal is reconstructed
                            animals.Add(new Animal(animalData[0], animalData[1], income));
                        }
                        else Console.WriteLine($"Invalid animal data on line {i + 1}. Skipping.");
                    }
                    UpdateIncome(); //Updates user income based on loaded animals
                    Console.WriteLine("Save data loaded successfully.");
                }
                else
                {
                    Console.WriteLine("Save file is empty. Starting fresh.");
                    userMoney = 0;
                }
            }
            else
            {
                Console.WriteLine("No save file found. Starting fresh.");
                userMoney = 0;
            }
        }
        static void Main(string[] args)
        {
            LoadSave();
            InitializeShop();
            DateTime lastIncomeTime = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - lastIncomeTime).TotalSeconds >= 1)
                { //Timed income system. Every second, the user receives money based on their animals
                    EarnedIncome();
                    lastIncomeTime = DateTime.Now;
                }
                Console.WriteLine();
                MainMenu();
                System.Threading.Thread.Sleep(100);
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    SaveExit();
                    break;
                }
            }
        }
    }
}

/*
 * Fix passive income system using a separate thread. Currently, income only updates when SaveExit() is called.
 * Fix SaveExit function and include "Saving . . ." animation using timers (Saving -> Saving . -> Saving . . -> Saving . . .), then end automatically close console window after animation.
 * Fix shop. The animal that is purchased is not the same one as the one that is displayed. I think the stack is reversed. Also, most expensive animals should give the most income
 * Animal naming .txt file
 * Try to incorporate inheritance or recursion, as well as data security features (public/private access modifiers, properties, interfaces)
 */