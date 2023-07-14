using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace pozdravliator
{
    internal static class Dialogs
    {
        static readonly string[] formatsOfDate = { "dd/MM/yyyy" /*, "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy"*/ };

        static public byte InputCommand(Dictionary<byte, string> commands, string title = "Available commands:")
        {
            Console.WriteLine(title);

            foreach (byte key in commands.Keys)
                Console.WriteLine($" {key} - {commands[key]}");
            Console.Write("\nInput the command: ");

            byte cmd;

            string? input;
            do
            {
                input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    try
                    {
                        cmd = Convert.ToByte(input);
                        if (commands.ContainsKey(cmd))
                            break;
                        else
                            Console.Write("Unknown command \"{0}\". Try again: ", input);
                    }
                    catch
                    {
                        Console.Write("Wrong format. Try again: ", input);
                    }
                }
            } while (true);

            return cmd;
        }

        static public DateTime InputDate(string[] formats)
        {
            DateTime date;
            string? input;

            Console.Write("Input date in format dd.MM.yyyy: ");
            do
            {
                input = Console.ReadLine();

                if (DateTime.TryParseExact(input, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out date))
                    break;
                else
                    Console.Write("Wrong format. Try again: ");
            } while (true);

            return date;
        }

        static public string InputPerson()
        {
            string person;
            string? input;

            Console.Write("Input name of person: ");
            do
            {
                input = Console.ReadLine();

                if ((input == null) || (input == ""))
                    Console.Write("Wrong format. Try again: ");
                else
                {
                    person = input;
                    break;
                }
            } while (true);

            return person;
        }

        static public int InputId()
        {
            int id;
            string? input;

            Console.Write("Input id of bday: ");
            do
            {
                input = Console.ReadLine();

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

        static public ListOfBirtdays AddBday(ListOfBirtdays bdays)
        {
            Console.WriteLine("--- Adding ---\n");

            ListOfBirtdays bdays_new = bdays;

            DateTime date = Dialogs.InputDate(formatsOfDate);
            string person = Dialogs.InputPerson();

            Console.WriteLine("\nNext bday will be added:");
            ListOfBirtdays.ShowBDay(null, date, person);
            Console.WriteLine();

            if (YesNoQuestion("Continue?"))
                bdays_new.Add(date, person);
            else
                Console.WriteLine("Canceled");
            Console.WriteLine();

            return bdays_new;
        }

        static public ListOfBirtdays DeleteBday(ListOfBirtdays bdays)
        {
            Console.WriteLine("--- Deleting ---\n");

            ListOfBirtdays bdays_new = bdays;

            int id = Dialogs.InputId();

            if (bdays[id] != null)
            {
                Console.WriteLine("Next bday will be deleted:");
                bdays.ShowBDay(bdays[id]);
                Console.WriteLine();

                if (YesNoQuestion("Continue?"))
                    bdays_new.Delete(id);
                else
                    Console.WriteLine("Canceled");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No such bday.\n");
            }
               
            return bdays_new;

        }

        static public ListOfBirtdays EditBday(ListOfBirtdays bdays)
        {
            Console.WriteLine("--- Editing ---\n");

            ListOfBirtdays bdays_new = bdays;

            int id = Dialogs.InputId();            

            if (bdays[id] != null)
            {
                Console.WriteLine("Next bday will be edited:");
                bdays.ShowBDay(bdays[id]);
                Console.WriteLine();

                Dictionary<byte, string> cmd_dict = new()
                {
                    { 0, "date" },
                    { 1, "name" }
                };

                byte cmd = Dialogs.InputCommand(cmd_dict, "Choose field:");

                DateTime? newDate = null;
                string? newPerson = null;
             
                
                switch (cmd)
                {
                    case 0:
                        newDate = Dialogs.InputDate(formatsOfDate);
                        Console.WriteLine("\nNext changes will be added:");
                        ListOfBirtdays.ShowBDay(id, (DateTime)newDate, bdays[id].Person);
                        break;
                    case 1:
                        newPerson = Dialogs.InputPerson();
                        Console.WriteLine("\nNext changes will be added:");
                        ListOfBirtdays.ShowBDay(id, bdays[id].Date, newPerson);
                        break;
                    default:
                        break;
                }

                Console.WriteLine();

                if (YesNoQuestion("Continue?"))
                    bdays_new.Edit(id, newDate, newPerson);
                else
                    Console.WriteLine("Canceled");
                Console.WriteLine();                
            }
            else
            {
                Console.WriteLine("No such bday.\n");
            }

            return bdays_new;
        }
    }
}
