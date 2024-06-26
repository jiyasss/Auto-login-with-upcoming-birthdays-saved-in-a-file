using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
namespace MyProgram
{
    public class User
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public partial class Programs
    {
        public static void Birthdays()
        {
            Console.Write("Enter the filename of BIRTHDAYS: ");
            string filename = Console.ReadLine();
            List<User> users = ReadUsersFromFile(filename);
            if (users != null)
            {
                List<User> upcomingBirthdays = FindUpcomingBirthdays(users, 10);

                if (upcomingBirthdays.Count > 0)
                {
                    Console.WriteLine("Birthdays within the next 10 days are listed:");
                    foreach (var user in upcomingBirthdays)
                    {
                        Console.WriteLine($"{user.Name}: {user.DateOfBirth.ToString("dd-MM-yyyy")}");
                    }
                }
                else
                {
                    Console.WriteLine("No upcoming birthdays within the next 10 days.");
                }
            }
            else
            {
                Console.WriteLine("Error!! Check your filename again.");
            }
        }

        static List<User> ReadUsersFromFile(string filename)
        {
            List<User> users = new List<User>();
            try
            {
                string[] lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        string name = parts[0];
                        DateTime dob;
                        if (DateTime.TryParseExact(parts[1], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            users.Add(new User { Name = name, DateOfBirth = dob });
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        static List<User> FindUpcomingBirthdays(List<User> users, int days)
        {
            DateTime today = DateTime.Today;
            DateTime upcomingDateLimit = today.AddDays(days);
            List<User> upcomingBirthdays = new List<User>();
            foreach (var user in users)
            {
                DateTime nextBirthday = new DateTime(today.Year, user.DateOfBirth.Month, user.DateOfBirth.Day);
                if (nextBirthday < today)
                {
                    nextBirthday = nextBirthday.AddYears(1);
                }
                if (nextBirthday <= upcomingDateLimit)
                {
                    upcomingBirthdays.Add(user);
                }
            }
            return upcomingBirthdays;
        }
    }

    
}
    
