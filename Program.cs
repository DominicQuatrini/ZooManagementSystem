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
        static Animal[] shopAnimals = new Animal[3];
        static decimal userMoney = 0;
        static decimal userIncome = 0;
        static DateTime lastIncomeTime;
        static void MainMenu()
        {
            Console.WriteLine("== Welcome to the Zoo Management System! == \na) Animal Database \nb) Shop \nc) Collect money \nd) Save and Exit");
            Console.Write($"\nCurrent Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{userMoney}");
            Console.ResetColor();

            Console.Write($"\nIncome: +");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${userIncome}"); //Prints current user money and income
            Console.ResetColor();
            Console.WriteLine(" per second");

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
        static void Database()
        {
            Console.WriteLine("== Welcome to the Database! ==");
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
            Console.WriteLine("\nPress enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu(); //Once the user hits the enter key, they are sent back to the main menu 
        }
        static void InitializeShop()
        {
            for (int i = 0; i < 3; i++)
            {
                shopAnimals[i] = Animal.RandomAnimal(i + 1);
                //Adds animals with varying exoticness to an array
            }
        }
        static void Shop()
        {
            Console.WriteLine($"== Welcome to the shop! ==");
            int basePrice = 0;
            for (int i = 0; i < shopAnimals.Length; i++)
            {
                Console.WriteLine($"{(char)('a' + i)}) {shopAnimals[i].name} the {shopAnimals[i].species}, ${basePrice + 50*(i + 1)}");
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
        static void PurchaseConfirmation(int price, string userChoice)
        {
            if (userMoney >= price)
            {
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
                    shopAnimals[2] = Animal.RandomAnimal(3);
                }
                userMoney -= price;
                animals.Add(purchasedAnimal); //Adds the purchased animal to the list of the user's animals
                UpdateIncome();
                Console.WriteLine($"You purchased {purchasedAnimal.name} the {purchasedAnimal.species} for ${price}!");
            }
            else Console.WriteLine("Not enough money to purchase this animal.");
            Console.ReadLine();
            Console.Clear();
            Shop();
        }
        static void FreeShop()
        {
            Animal firstAnimal = new Animal("Rainier", "Rhino", 3);
            Console.WriteLine($"== Welcome to the shop! ==" +
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
                    Console.WriteLine($"You purchased {firstAnimal.name} the {firstAnimal.species}! \nPress enter to go back to the main menu.");
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
        static void UpdateIncome()
        { //Adds up all of the income from the user's animals, to be received per second. When called, updates the user's income to include newly purchased animals
            userIncome = animals.Sum(a => a.income);
        }
        static void SaveExit()
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
        static string SaveInfo()
        {
            StringBuilder saveInfo = new StringBuilder();
            saveInfo.AppendLine(userMoney.ToString());
            foreach (Animal animal in animals)
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
                        Console.WriteLine("Invalid format in save file. Resetting user money to 0.\n");
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
                    Console.WriteLine("Save data loaded.\n");
                }
                else
                {
                    Console.WriteLine("Save file is empty. Starting fresh.\n");
                    userMoney = 0;
                }
            }
            else
            {
                Console.WriteLine("No save file found. Starting fresh.\n");
                userMoney = 0;
            }
        }
        static void CollectMoney()
        {
            int secondsElapsed = (int)Math.Round((DateTime.Now - lastIncomeTime).TotalSeconds);
            userMoney += userIncome * secondsElapsed;
            Console.Write($"You earned ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${userIncome * secondsElapsed} ");
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
 * Animal naming .txt file
 * Try to incorporate inheritance or recursion, as well as data security features (public/private access modifiers, properties, interfaces)
 */