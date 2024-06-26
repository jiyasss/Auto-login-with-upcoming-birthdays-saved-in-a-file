using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Win32;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace MyProgram
{
    public class NumberValidator
    {
        public static int number(string num)
        {
            int a;
            while (!int.TryParse(num, out a))
            {
                Console.WriteLine("Please Only Enter Number");
                num = Console.ReadLine();
            }
            return a;
        }
    }
    public partial class Programs
    {
        static string username;
        static string password;
        static bool login = false;

        public static string directoryPath;
        public static string contentFilePath;

        static void SaveToFile()
        {
            string pathTo = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(pathTo).Parent.Parent.Parent;
            directoryPath = Path.Combine(Dinfo.FullName, "output");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            contentFilePath = Path.Combine(directoryPath, "content.txt");

        }
        public static void Options()
        {
            SaveToFile();

            bool continueprocess = true;

            while (continueprocess)
            {
                Console.WriteLine("PRESS 1 : LOGIN AGAIN");
                Console.WriteLine("PRESS 2 : CREATE NEW USER");
                Console.WriteLine("PRESS 3 : EXIT");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RegisterAndLogin();
                            break;
                        case 2:
                            CreateNewUser();
                            break;
                        case 3:
                            continueprocess = false;
                            Console.WriteLine("Exiting , bye-bye xxx");

                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
        static void RegisterAndLogin()
        {
            Thread registerThread = new Thread(RegisterUser);
            Thread loginThread = new Thread(LoginUser);

            registerThread.Start();
            loginThread.Start();

            registerThread.Join();
            loginThread.Join();

            if (login)
            {
                
                DisplayWelcome();
                Birthdays();


            }

        }
        static void LoginUser()
        {
            if (!string.IsNullOrEmpty(username))
            {
                login = true;
            }
            else
            {
                Console.WriteLine("No user registered.");
            }
        }
        static void DisplayWelcome()
        {
            Console.Clear();
            Console.WriteLine("WELCOME" + username);
            using (StreamWriter writer = File.AppendText(contentFilePath))
            {
                writer.WriteLine($"{username},{password}");
            }
            while (login)
            {
                Console.WriteLine("Press 1 : LOGOUT.");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    login = false;
                    Console.WriteLine("LOGGED OUT");
                    
                    DisplayWelcome();

                }

            }

        }
        static void RegisterUser()
        {
            username = GenerateEmail();
            password = GeneratePassword();
            Console.WriteLine("new user created with email" + username);
        }


        static void LogInAgain()
        {
            Thread loginagain = new Thread(LoginUser);
            loginagain.Start();
            loginagain.Join();

            if (login)
            {
                DisplayWelcome();
            }
        }
        static void CreateNewUser()
        {
            username = GenerateEmail();
            password = GeneratePassword();
            Console.WriteLine("new user created with email" + username);
            login = true;
            DisplayWelcome();
        }
        static Random rand = new Random();
        static int emailCounter = 1;

        static string GenerateEmail()
        {
            string email = "jiya" + emailCounter.ToString("D2") + "@gmail.com";
            emailCounter++;
            return email;
        }


        static string GeneratePassword()
        {
            string allowedChars = "0123456789";
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                password.Append(allowedChars[random.Next(allowedChars.Length)]);
            }
            return password.ToString();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Programs.Options();
            Console.ReadKey();
        }
    }
}
