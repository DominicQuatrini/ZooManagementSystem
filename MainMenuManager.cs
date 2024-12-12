using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class MainMenuManager
    {
        internal static List<Animal> userAnimals = new List<Animal>();
        private static void DisplayMainMenu()
        {
            Console.WriteLine("== Welcome to the Zoo Management System! == \na) Animal Database \nb) Shop \nc) Collect money \nd) Save and Exit");
            Console.Write($"\nCurrent Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${ShopManager.UserMoney}");
            Console.ResetColor();

            Console.Write($"\nIncome: +");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"${ShopManager.UserIncome}");
            Console.ResetColor();
            Console.WriteLine("/s");
        }
        private static void HandleMainMenuChoice(string menuChoice) //Calls the corresponding method based on the user's input
        {
            switch (menuChoice)
            {
                case "a":
                    Database();
                    break;
                case "b":
                    if (userAnimals.Count == 0) FreeShopManager.FreeShop();
                    else ShopManager.Shop();
                    break;
                case "c":
                    ShopManager.CollectMoney();
                    break;
                case "d":
                    SaveManager.Save();
                    break;
                default:
                    break;
            }
        }
        private static void Database()
        {
            Console.WriteLine("== Welcome to the Database! ==");
            if (userAnimals.Count == 0) Console.WriteLine("You don't own any animals. Visit the shop.");
            else
            {
                for (int i = 0; i < userAnimals.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    if (userAnimals[i].IsMythical == true) userAnimals[i].DisplayAnimal();
                    else userAnimals[i].DisplayAnimal();
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\nPress enter to return to the main menu.");
            Console.ReadLine();
            MainMenu();
        }
        internal static void MainMenu()
        {
            Console.Clear();
            DisplayMainMenu();
            string menuChoice = Program.GetUserChoice(4);
            Console.Clear();
            HandleMainMenuChoice(menuChoice);
        }
    }
}
//BUG: When Database is ran multiple times in a row, fragments of it appear in the console