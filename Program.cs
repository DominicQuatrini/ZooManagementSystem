using System.Text;

namespace ZooManagementSystem
{
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
        internal static string GetUserChoice(int numberOfOptions)
        {
            string userChoice = Console.ReadLine().ToLower().Trim();
            List<string> choices = new List<string>();

            for (int i = 0; i < numberOfOptions; i++) choices.Add($"{(char)('a' + i)}");
            while(choices.Contains(userChoice) == false)
            {
                Console.WriteLine("Please enter an option above.");
                userChoice = Console.ReadLine().ToLower().Trim();
            }
            return userChoice;
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
 * Break up PurchaseConfirmation() into multiple methods, each with only a single objective
 * Include mythical data in save.txt, so that the user may only purchase 1 of each mythical. Currently, the mythical array resets to normal after the user reopens the program, meaning users can get more than 7 mythicals
 */