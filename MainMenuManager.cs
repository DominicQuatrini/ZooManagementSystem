using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class MainMenuManager
    {
        private static void DisplayMainMenu()
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
            string menuChoice = Program.GetUserChoice(4);
            Console.Clear();
            HandleMainMenuChoice(menuChoice);
        }
    }
}
