using System.Text;

namespace ZooManagementSystem
{
    internal class Program
    {
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
            ShopManager.lastIncomeTime = DateTime.Now;
            MainMenuManager.MainMenu();
        }
    }
}
/*
 * Abstraction/Interfaces
 * Include mythical data in save.txt, so that the user may only purchase 1 of each mythical. Currently, the mythical array resets to normal after the user reopens the program, meaning users can get more than 7 mythicals
 */