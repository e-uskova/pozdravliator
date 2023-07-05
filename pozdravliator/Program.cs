using System;
using System.Globalization;


/*
 * Приложение "Поздравлятор". Функциональность приложения - ведение списка дней
рождения (далее ДР) друзей / знакомых / сотрудников, а именно:
• Отображение всего списка ДР (дополнительные возможности, такие как сортировка,
выделение текущих и просроченных и т.п. - остаются на усмотрение реализующего)
• Отображение списка сегодняшних и ближайших ДР (дополнительные возможности,
такие как сортировка, выделение текущих и просроченных и т.п. - остаются на усмотрение
реализующего)
• Добавление записей в список ДР
• Удаление записей из списка ДР
• Редактирование записей в списке ДР
*/

namespace pozdravliator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Birthday> bdays = new();

            bdays.Add(new Birthday(new DateTime(2000, 5, 2), "Kate"));
            bdays.Add(new Birthday(new DateTime(1998, 7, 17), "Lana"));
            bdays.Add(new Birthday(new DateTime(1997, 1, 29), "Alex"));
            bdays.Add(new Birthday(new DateTime(2003, 5, 15), "Rina"));
            bdays.Add(new Birthday(new DateTime(2022, 9, 6), "Shkiper"));
            bdays.Add(new Birthday(new DateTime(2023, 7, 4), "This code"));
            bdays.Add(new Birthday(DateTime.Today, "Today"));
            bdays.Add(new Birthday(DateTime.Today.AddYears(-5), "Today 5 year ago"));

            Console.WriteLine("*** Welcome to Pozdravliator! ***\n");

            byte cmd = 1;

            while (cmd != 0)
            {

                switch (cmd)
                {
                    case 1:
                        ShowAllBDays(bdays);
                        break;
                    case 2:
                        ShowTodayBDays(bdays);
                        break;
                    case 3:
                        ShowNearestBDays(bdays);
                        break;
                    case 4:
                        bdays = AddBDay(bdays);
                        break;
                    case 5:
                        bdays = DeleteBDay(bdays);
                        break;
                    case 6:
                        bdays = EditBDay(bdays);
                        break;
                    default:
                        break;
                }

                
                Console.WriteLine("Available commands:\n" +
                                  "1 - show all bdays\n" +
                                  "2 - today bdays\n" +
                                  "3 - nearest bdays\n" +
                                  "4 - add bday\n" +
                                  "5 - delete bday\n" +
                                  "6 - edit bday\n" +
                                  "0 - exit\n");

                Console.WriteLine("Input the command: ");

                List<string> commands = new() { "0", "1", "2", "3", "4", "5", "6"};
                bool wrongInput = true;
                string? input;
                do
                {
                    input = Console.ReadLine();
                    if (input != null)
                    {
                        input = input.Trim();
                        if (commands.Contains(input))
                        {
                            cmd = Convert.ToByte(input);
                            wrongInput = false;
                        }
                        else
                            Console.WriteLine("Unknown command \"{0}\". Try again:", input);
                    }
                } while (wrongInput);

                Console.Clear();

            }


        }

        static void ShowBDay(Birthday bday)
        {
            Console.WriteLine($" {bday.Id, -5} | {bday.Date, -8:dd/MM/yy} | {bday.Person}");
        }        
        
        static void ShowAllBDays(List<Birthday> bdays)
        {
            Console.WriteLine("--- All Birthdays ---\n");
            Console.WriteLine(" ID    | Date     | Person\n" +
                              "------------------------------");
            foreach (var b in bdays)
                ShowBDay(b);
            Console.WriteLine();
        }
        static void ShowTodayBDays(List<Birthday> bdays)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine("--- Today Birthdays ---\n");
            Console.WriteLine(" ID    | Date     | Person\n" +
                              "------------------------------");
            foreach (var b in bdays)
            {
                DateTime thisYearDate = new(today.Year, b.Date.Month, b.Date.Day);
                if (thisYearDate == today)
                    ShowBDay(b);
            }
               
            Console.WriteLine();
        }
        static void ShowNearestBDays(List<Birthday> bdays, ushort period = 90)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine("--- Nearest Birthdays ---\n");
            Console.WriteLine(" ID    | Date     | Person\n" +
                              "------------------------------");

/*            ushort counter = 0;*/
            foreach (var b in bdays)
            { 
                DateTime thisYearDate = new(today.Year, b.Date.Month, b.Date.Day);
                DateTime nextDate;
                if ((thisYearDate - today).TotalDays < 0)
                    nextDate = new(today.Year + 1, b.Date.Month, b.Date.Day);                
                else
                    nextDate = thisYearDate;


                if (thisYearDate == today || (nextDate - today).TotalDays < period)
                {
                    ShowBDay(b);
/*                    counter++;*/
                }
            }   
            Console.WriteLine();
        }

        static List<Birthday> AddBDay(List<Birthday> bdays)
        {
            DateTime date;
            string person;

            string? input;

            Console.WriteLine("Input date in format dd.MM.yyyy:");
            do
            {
                input = Console.ReadLine();
                string[] formats = { "dd/MM/yyyy"/*, "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy" */};

                if (DateTime.TryParseExact(input, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out date))                
                    break;
                else
                    Console.WriteLine("Wrong format. Try again:");                    
            } while (true);

            Console.WriteLine("Input name of person:");
            do
            {
                input = Console.ReadLine();

                if ((input == null) || (input == ""))
                    Console.WriteLine("Wrong format. Try again:");
                else
                {
                    person = input;
                    break;
                }
            } while (true);

            List<Birthday> bdays_new = bdays;
            bdays_new.Add(new(date, person));

            Console.WriteLine("Bithday added.\n");

            return bdays_new;
        }

        static List<Birthday> DeleteBDay(List<Birthday> bdays)
        {
            ushort id;
            string? input;

            Console.WriteLine("Input id of bday you want to delete:");
            do
            {
                input = Console.ReadLine();

                try
                {
                    id = Convert.ToUInt16(input);
                    break;
                }
                catch
                {
                    Console.WriteLine("Wrong format. Try again:");
                }
            } while (true);

            List<Birthday> bdays_new = new();
            foreach (Birthday bday in bdays)
            {
                if (bday.Id != id)
                    bdays_new.Add(bday);
            }

            Console.WriteLine("Bithday deleted.\n");

            return bdays_new;
        }

        static List<Birthday> EditBDay(List<Birthday> bdays)
        {
            ushort id;
            string? input;

            List<Birthday> bdays_new = bdays;            
            
            Console.WriteLine("Input id of bday you want to edit:");
            do
            {
                input = Console.ReadLine();

                try
                {
                    id = Convert.ToUInt16(input);
                    break;
                }
                catch
                {
                    Console.WriteLine("Wrong format. Try again:");
                }
            } while (true);

            Console.WriteLine("Which value?\n" +
                              "d - date\n" +
                              "n - name\n");
            string[] commands = { "d", "n" };
            do
            {
                input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    if (commands.Contains(input))
                    {
                        string cmd = input;
                        switch (cmd)
                        {
                            case "d":
                                DateTime newDate = InputDate();
                                foreach (Birthday bday in bdays_new)
                                {
                                    if (bday.Id == id)
                                        bday.Edit(date: newDate);
                                }
                                break;
                            case "n":
                                string newName = InputPerson();
                                foreach (Birthday bday in bdays_new)
                                {
                                    if (bday.Id == id)
                                        bday.Edit(person: newName);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unknown command \"{0}\". Try again:", input);
                    }
                }
            } while (true);

            Console.WriteLine("Bithday edited.\n");

            return bdays_new;
        }

        static DateTime InputDate()
        {
            DateTime date;
            string? input;

            Console.WriteLine("Input date in format dd.MM.yyyy:");
            do
            {
                input = Console.ReadLine();
                string[] formats = { "dd/MM/yyyy"/*, "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy" */};

                if (DateTime.TryParseExact(input, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out date))
                    break;
                else
                    Console.WriteLine("Wrong format. Try again:");
            } while (true);

            return date;
        }
        static string InputPerson()
        {
            string person;
            string? input;

            Console.WriteLine("Input name of person:");
            do
            {
                input = Console.ReadLine();

                if ((input == null) || (input == ""))
                    Console.WriteLine("Wrong format. Try again:");
                else
                {
                    person = input;
                    break;
                }
            } while (true);

            return person;
        }

    }
}