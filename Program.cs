﻿using System.ComponentModel;
using System.Xml.Serialization;

namespace ZooManagementSystem
{
    class Animal
    {
        public string name; public string species; public int money;
        public Animal(string name, string species, int money)
        {
            this.name = name;
            this.species = species;
            this.money = money;
        }
        public void DisplayAnimal()
        {
            Console.WriteLine($"{name} the {species}, $/s: {money}");
        }
    }
    class Program
    {
        static List<Animal> animals = new List<Animal>
            {
            };
        //creates a list of all of the user's animals within the class scope. Purchased animals will be added to this list
        static void MainMenu()
        {
            Console.WriteLine("Welcome to the Zoo Management System! Please choose an option. \na) Animal Database \nb) Shop \nc) Save and Exit");
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            Console.Clear();
            //Prompts user for input. If they enter anything other than A, a, B, b, C, c, then it will return an error message and prompt them to enter another option.
            switch (menuChoice)
            {
                case "a":
                    Database();
                    break;
                case "b":
                    Shop();
                    break;
                case "c":
                    SaveExit();
                    break;
                default:
                    break;
            }
            //Calls the function that corresponds to the user's input
        }
        static void Database()
        {
            if (animals.Count == 0)
                Console.WriteLine("You don't own any animals. Visit the shop.");
            //If the list of animals is empty, the user is prompted to visit the shop.
            else
            {
                for (int i = 0; i < animals.Count; i++)
                {
                    Console.Write($"{i}. ");
                    animals[i].DisplayAnimal();
                }
            }
            //If the user has animals, it displays their name, species, and money per second
            Console.WriteLine("Press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu();
            //Once the user hits the enter key, they are sent back to the main menu
        }
        static void Shop()
        {
            Console.WriteLine("Welcome to the shop!\na) Rainier the Rhino\nb) \nc) \nd) Back to main menu");
            //new Animal("Rainier", "Rhino", 3),
            //new Animal("George", "Monkey", 1),
            //new Animal("James", "Peach", 0)
            string menuChoice = Console.ReadLine().ToLower();
            while (menuChoice != "a" && menuChoice != "b" && menuChoice != "c" && menuChoice != "d")
            {
                Console.Write("Please input one of the options listed above.");
                menuChoice = Console.ReadLine().ToLower();
            }
            //Once again, requires the user to enter A, a, B, b, C, c, D, d to continue. Else, they are prompted to input a different option
            Console.Clear();
            switch (menuChoice)
            {
                case "a":
                    PurchaseConfirmation();
                    break;
                case "b":
                    PurchaseConfirmation();
                    break;
                case "c":
                    PurchaseConfirmation();
                    break;
                case "d":
                    MainMenu();
                    break;
                default:
                    break;
            }
        }
        static void PurchaseConfirmation()
        {
            //Confirms the purchase, removes money from user's account and adds animal to the list of animals
            ShopRestock();
        }
        static void ShopRestock()
        {
            //Once an animal is purchased, a new, randomized one should take its place in the shop.
        }
        static void SaveExit()
        {
            string saveInfo = SaveInfo();
            File.WriteAllText("save.txt", saveInfo);
            //Writes the returned value of saveInfo to a .txt file

            Console.WriteLine("Saving . . .");
            Console.WriteLine("Game successfully saved!");
        }
        static string SaveInfo()
        {
            string saveInfo = "";
            for (int i = 0; i < animals.Count; i++)
            {
                saveInfo += $"{animals[i].name}\n{animals[i].species}\n{animals[i].money}\n";
            }
            //Saves all animal data to one string, with several lines
            return saveInfo;
        }
        static void LoadSave()
        {
            string line;
            int loadCount = 0;
            if (File.Exists("save.txt"))
            {
                using (StreamReader sr = new StreamReader("save.txt"))
                {
                    //Reads save.txt line by line and reconstructs the animal objects
                    while ((line = sr.ReadLine()) != null)
                    {
                        string name = line.Trim();

                        line = sr.ReadLine();
                        string species = line.Trim();

                        line = sr.ReadLine();
                        string mStr = line.Trim();
                        int money = int.Parse(mStr);

                        animals.Add(new Animal(name, species, money));
                        Console.Write($"Loaded: {name} the {species}, $/s: {money}");
                        loadCount++;
                    }
                }
            }
            if (loadCount == 0 || File.Exists("save.txt") == false)
                Console.WriteLine("No saved animals found.\n");
            else
                Console.WriteLine("Loading complete.\n");
        }
        static void Main(string[] args)
        {
            LoadSave();
            MainMenu();
        }
    }
}

/*
- randomized animals in shop for set prices (3 options at a time, 1 small/cheap, 1 medium, 1 large/expensive)
    - when an animal is bought, another randomized animal should take its place in the shop
    - animal names randomized from a .txt file of a bunch of different options
    - use enums for animal species randomization
- implement money system
    - user should passively gain money from animals they own (the sum of all the owned animals' money field, per second)
    - shop should accurately remove money from user's account
*/