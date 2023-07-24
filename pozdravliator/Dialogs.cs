using System.Globalization;

namespace pozdravliator
{
    internal static class Dialogs
    {
        static readonly string[] formatsOfDate = { "dd/MM/yyyy" /*, "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy"*/ };

        static public byte? InputCommand(Dictionary<byte, string> commands, string title = "Available commands:", bool nullAvailable = false)
        {
            /*Console.WriteLine("----------------------------");*/
            Console.WriteLine(title);

            foreach (byte key in commands.Keys)
                Console.WriteLine($" {key} - {commands[key]}");
            if (nullAvailable)
                Console.WriteLine("You can use empty input to interrupt operation.");
            /*Console.WriteLine("----------------------------");*/
            Console.Write("\nInput the command: ");

            byte cmd;

            string? input;
            do
            {
                input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    if (nullAvailable && input == "")
                        return null;
                    try
                    {
                        cmd = Convert.ToByte(input);
                        if (commands.ContainsKey(cmd))
                            return cmd;
                        else
                            Console.Write("Unknown command \"{0}\". Try again: ", input);
                    }
                    catch
                    {
                        Console.Write("Wrong format. Try again: ");
                    }
                }
            } while (true);            
        }

        static public DateTime? InputDate(string[] formats)
        {
            string? input;

            Console.WriteLine("You can use empty input to interrupt operation.");
            Console.Write("Input date in format dd.MM.yyyy: ");
            do
            {
                input = Console.ReadLine();
                if (input != null && input.Trim() == "")
                    return null;
                else if (DateTime.TryParseExact(input, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime date))
                    return date;
                else
                    Console.Write("Wrong format. Try again: ");
            } while (true);
        }

        static public string? InputPerson()
        {
            string? input;

            Console.WriteLine("You can use empty input to interrupt operation.");
            Console.Write("Input name of person: ");
            do
            {
                input = Console.ReadLine();
                if (input != null && input.Trim() == "")
                    return null;
                else // input is null or not empty
                    return input;
            } while (true);            
        }

        static public int? InputId()
        {
            int id;
            string? input;

            Console.WriteLine("You can use empty input to interrupt operation."); 
            Console.Write("Input id of bday: ");
            do
            {
                input = Console.ReadLine();
                if (input != null && input.Trim() == "")
                    return null;

                try
                {
                    id = Convert.ToInt32(input);
                    break;
                }
                catch
                {
                    Console.Write("Wrong format. Try again: ");
                }
            } while (true);

            return id;
        }

        static public bool YesNoQuestion(string text)
        {
            Dictionary<byte, string> commands = new()
            {
                {0, "no" },
                {1, "yes"}
            };

            return (InputCommand(commands, text) == 1);
        }

        static public void ShowBDay(Birthday? bday)
        {
            if (bday != null)
                Console.WriteLine($" {bday.Id,3} | {bday.Date,-8:dd/MM/yy} | {bday.Person}");
        }

        static public void ShowBDay(int? id, DateTime date, string person)
        {
            if (id == null)
                Console.WriteLine($"     | {date,-8:dd/MM/yy} | {person}");
            else
                Console.WriteLine($" {id,3} | {date,-8:dd/MM/yy} | {person}");
        }

        static private string SectionTitle(string title)
        {
            return $"--- {title} ---\n";
        }

        static private string tableHeader = "  ID | Date     | Person\n" +
                                            "----------------------------";

        static public void ShowAllBDays(ListOfBirtdays bdays)
        {
            Console.WriteLine(SectionTitle("All Birthdays"));
            Console.WriteLine(tableHeader);

            foreach (var b in bdays)
                ShowBDay(b);
            Console.WriteLine();
        }

        static public void ShowTodayBDays(ListOfBirtdays bdays)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine(SectionTitle("Today Birthdays"));
            Console.WriteLine(tableHeader);

            foreach (var b in bdays)
            {
                DateTime thisYearDate = new(today.Year, b.Date.Month, b.Date.Day);
                if (thisYearDate == today)
                    ShowBDay(b);
            }

            Console.WriteLine();
        }

        static public void ShowNearestBDays(ListOfBirtdays bdays, ushort period = 90)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine(SectionTitle("Nearest Birthdays"));
            Console.WriteLine(tableHeader);

            /*ushort counter = 0;*/
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
                    /*counter++;*/
                }
            }
            Console.WriteLine();
        }

        static public ListOfBirtdays AddBday(ListOfBirtdays bdays)
        {
            Console.WriteLine(SectionTitle("Adding"));

            ListOfBirtdays bdays_new = bdays;

            DateTime? date = InputDate(formatsOfDate);
            if (date != null)
            {
                string? person = InputPerson();
                if (person != null) 
                {
                    Console.WriteLine("\nNext bday will be added:");
                    ShowBDay(null, (DateTime)date, person);
                    Console.WriteLine();

                    if (YesNoQuestion("Continue?"))
                        bdays_new.Add((DateTime)date, person);
                    else
                        Console.WriteLine("Canceled");
                }
                else
                {
                    Console.WriteLine("Interrupted");
                }
            }
            else
            {
                Console.WriteLine("Interrupted");
            }
            Console.WriteLine();
            return bdays_new;
        }

        static public ListOfBirtdays DeleteBday(ListOfBirtdays bdays)
        {
            Console.WriteLine(SectionTitle("Deleting"));

            ListOfBirtdays bdays_new = bdays;

            int? id = InputId();
            if (id != null)
            {
                if (bdays[(int)id] != null)
                {
                    Console.WriteLine("Next bday will be deleted:");
                    ShowBDay(bdays[(int)id]);
                    Console.WriteLine();

                    if (YesNoQuestion("Continue?"))
                        bdays_new.Delete((int)id);
                    else
                        Console.WriteLine("Canceled");
                }
                else
                {
                    Console.WriteLine("No such bday.");
                }
            }
            else
            {
                Console.WriteLine("Interrupted");
            }
            Console.WriteLine();            
            return bdays_new;
        }

        static public ListOfBirtdays EditBday(ListOfBirtdays bdays)
        {
            Console.WriteLine(SectionTitle("Editing"));

            ListOfBirtdays bdays_new = bdays;

            int? id = InputId();
            if (id != null)
            {
                Birthday? bday = bdays[(int)id];
                if (bday != null) 
                {
                    Console.WriteLine("Next bday will be edited:");
                    ShowBDay(bday);
                    Console.WriteLine();

                    Dictionary<byte, string> cmd_dict = new()
                    {
                        { 0, "date" },
                        { 1, "name" }
                    };

                    byte? cmd = InputCommand(cmd_dict, "Choose field:", true);
                    if (cmd != null)
                    {
                        DateTime? newDate = null;
                        string? newPerson = null;             
                
                        switch (cmd)
                        {
                            case 0:
                                newDate = InputDate(formatsOfDate);
                                if (newDate != null)
                                {
                                    Console.WriteLine("\nNext changes will be added:");
                                    ShowBDay(id, (DateTime)newDate, bday.Person);
                                }
                                else
                                {
                                    Console.WriteLine("Interrupted");
                                }
                                break;
                            case 1:
                                newPerson = InputPerson();
                                if (newPerson != null)
                                {
                                    Console.WriteLine("\nNext changes will be added:");
                                    ShowBDay(id, bday.Date, newPerson);
                                }
                                else
                                {
                                    Console.WriteLine("Interrupted");
                                }
                                break;
                        }

                        Console.WriteLine();

                        if (YesNoQuestion("Continue?"))
                        {
                            bdays_new.Edit((int)id, newDate, newPerson);
                            Console.WriteLine("Edited");
                        }
                        else
                            Console.WriteLine("Canceled");
                    }
                    else
                    {
                        Console.WriteLine("Interrupted");
                    }              
                }
                else
                {
                    Console.WriteLine("No such bday.");
                }
            }
            else
            {
                Console.WriteLine("Interrupted");
            }
            Console.WriteLine();  
            return bdays_new;
        }
    }
}
