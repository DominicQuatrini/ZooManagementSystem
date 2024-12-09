﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class ShopManager
    {
        internal static Animal[] shopAnimals = new Animal[3];
        private static int priceA = 50;
        private static int factorB = 5; private static int priceB = priceA * factorB; //Declare price equations to prevent redundancy
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
            string menuChoice = Program.GetUserChoice(4);
            HandleShopChoice(menuChoice);
        }
    }
    internal class FreeShopManager
    {
        internal static Animal firstAnimal = new Animal("Rainier", "Rhino", 3, false);
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

            string menuChoice = Program.GetUserChoice(2);
            HandleFreeShopChoice(menuChoice);
        }
    }
}
