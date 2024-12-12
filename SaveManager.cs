using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooManagementSystem
{
    internal class SaveManager
    {
        private static void ExitAnimation() //Prints "Saving" -> "Saving ." -> "Saving . ." ->  "Saving . . ." with intervals between each update
        {
            DateTime temp = DateTime.Now;
            double delay = 0.75;
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
        private static string SaveInfo()
        {
            StringBuilder saveInfo = new StringBuilder(); //Must use StringBuilder because strings are immutable after initialized
            saveInfo.AppendLine(ShopManager.UserMoney.ToString());
            foreach (Animal animal in MainMenuManager.userAnimals)
                saveInfo.AppendLine($"{animal.Name},{animal.Species},{animal.Income},{animal.IsMythical}");
            return saveInfo.ToString();
        }
        internal static void Save()
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
                ShopManager.UserMoney = money;
            else
            {
                Console.WriteLine("Invalid format in save file. Resetting user money to 0.\n");
                ShopManager.UserMoney = 0;
            }
        }
        private static void LoadAnimals(string[] lines) //Reconstructs animals from animal data. If data is improperly formatted, an error message is displayed and reconstruction is skipped
        {
            SpecialAnimal temp;
            for (int i = 1; i < lines.Length; i++)
            {
                string[] animalData = lines[i].Split(',');
                if (animalData.Length == 4 && int.TryParse(animalData[2], out int income))
                {
                    if (bool.TryParse(animalData[3], out bool isMythical))
                    {
                        if (isMythical)
                        {
                            temp = new SpecialAnimal(animalData[0], animalData[1], income, true);
                            MainMenuManager.userAnimals.Add(temp);
                            SpecialAnimal.specialAnimals.Remove(temp);
                        }
                        else MainMenuManager.userAnimals.Add(new Animal(animalData[0], animalData[1], income, false));
                    }
                    else Console.WriteLine($"Invalid animal isMythical data on line {i + 1}. Skipping.");
                }
                else Console.WriteLine($"Invalid animal income data on line {i + 1}. Skipping.");
            }
        }
        internal static void LoadSave()
        {
            if (File.Exists("save.txt"))
            {
                string[] lines = File.ReadAllLines("save.txt");
                if (lines.Length > 0)
                {
                    LoadMoney(lines);
                    LoadAnimals(lines);
                    ShopManager.UpdateIncome();
                    Console.WriteLine("Save data loaded. Press enter to continue.");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Save file is empty or could not be found. Press enter to continue.");
            ShopManager.UserMoney = 0;
            Console.ReadLine();
        }
    }
}
