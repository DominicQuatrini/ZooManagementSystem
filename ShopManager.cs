using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class ShopManager
    {
        private static Animal[] shopAnimals = new Animal[3];
        private static int priceA = 50;
        private static int factorB = 5; private static int priceB = priceA * factorB; //Declare price equations to prevent redundancy
        private static int factorC = 10; private static int priceC = priceA * factorC;
        private static int factorM = 20; private static int priceM = priceA * factorM;

        internal static decimal UserMoney { get; set; }
        internal static decimal UserIncome { get; set; }
        internal static DateTime lastIncomeTime;
        internal static void CollectMoney()
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
            UserIncome = MainMenuManager.userAnimals.Sum(a => a.Income);
        }
        internal static void InitializeShop() //Creates the first set of animals in the shop when the program is executed.
        {
            for (int i = 0; i < 3; i++) shopAnimals[i] = Animal.RandomAnimal(i);
        }
        private static void DisplayShopOffers(int i) //Only one shop offer prints each time DisplayShopOffers(int i) is called to allow the use of recursion
        {
            if (i >= shopAnimals.Length) return; //Stops the recursion if i is out of bounds for shopAnimals[i]

            Console.Write($"{(char)('a' + i)}) ");
            shopAnimals[i].DisplayAnimal();

            if (shopAnimals[i].IsMythical == true) Console.WriteLine($", ${priceM}");
            else if (i == 0) Console.WriteLine($", ${priceA}");
            else if (i == 1) Console.WriteLine($", ${priceB}");
            else Console.WriteLine($", ${priceC}");

            DisplayShopOffers(i + 1);
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
        private static void PurchaseConfirmation(int price, string userChoice)
        {
            Console.Clear();
            if (UserMoney >= price)
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
                UserMoney -= price;
                MainMenuManager.userAnimals.Add(purchasedAnimal);
                UpdateIncome();
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
            string menuChoice = Program.GetUserChoice(4);
            HandleShopChoice(menuChoice);
        }
    }
    internal class FreeShopManager //Serves as an easy solution to the issue of the user not being able to do anything when first starting due to no animals and no money
    {
        private static Animal firstAnimal = new Animal("Rainier", "Rhino", 3, false);
        private static void HandleFreeShopChoice(string menuChoice)
        {
            switch (menuChoice)
            {
                case "a":
                    MainMenuManager.userAnimals.Add(firstAnimal);
                    ShopManager.UpdateIncome();
                    Console.WriteLine($"You received {firstAnimal.Name} the {firstAnimal.Species}! \nPress enter to go back to the main menu.");
                    Console.ReadLine();
                    MainMenuManager.MainMenu();
                    break;
                case "b":
                    MainMenuManager.MainMenu();
                    break;
                default: break;
            }
        }
        internal static void FreeShop()
        {

            Console.WriteLine($"== Welcome to the shop! ==" +
                $"\na) {firstAnimal.Name} the {firstAnimal.Species}, FREE" +
                $"\nb) Back to main menu");

            string menuChoice = Program.GetUserChoice(2);
            HandleFreeShopChoice(menuChoice);
        }
    }
}
