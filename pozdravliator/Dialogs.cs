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

        static public int InputId(/*ListOfBirtdays bdays*/)
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
    }
}
