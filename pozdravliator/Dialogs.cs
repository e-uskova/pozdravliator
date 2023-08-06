using System.Globalization;

namespace pozdravliator
{
    internal static class Dialogs
    {
        static readonly string[] formatsOfDate = { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy" };

        static public byte? InputCommand(Dictionary<byte, string> commands, string title = "Доступные команды:", bool nullAvailable = false)
        {
            /*Console.WriteLine("----------------------------");*/
            Console.WriteLine(title);

            foreach (byte key in commands.Keys)
                Console.WriteLine($" {key} - {commands[key]}");
            if (nullAvailable)
                Console.WriteLine("- Для прерывания операции используйте пустой ввод. -");
            /*Console.WriteLine("----------------------------");*/
            Console.Write("\nВведите команду: ");

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
                            Console.Write("Неизвестная команда \"{0}\". Повторите попытку: ", input);
                    }
                    catch
                    {
                        Console.Write("Некорректный формат. Повторите попытку: ");
                    }
                }
            } while (true);            
        }

        static public DateTime? InputDate(string[] formats)
        {
            string? input;

            Console.WriteLine("- Для прерывания операции используйте пустой ввод. -");
            Console.Write("Введите дату в формате день.месяц.год: ");
            do
            {
                input = Console.ReadLine();
                if (input != null && input.Trim() == "")
                    return null;
                else if (DateTime.TryParseExact(input, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime date))
                    return date;
                else
                    Console.Write("Некорректный формат. Повторите попытку: ");
            } while (true);
        }

        static public string? InputPerson()
        {
            string? input;

            Console.WriteLine("- Для прерывания операции используйте пустой ввод. -");
            Console.Write("Введите имя: ");
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

            Console.WriteLine("- Для прерывания операции используйте пустой ввод. -"); 
            Console.Write("Введите ID записи: ");
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
                    Console.Write("Некорректный формат. Повторите попытку: ");
                }
            } while (true);

            return id;
        }

        static public bool YesNoQuestion(string text)
        {
            Dictionary<byte, string> commands = new()
            {
                {0, "нет" },
                {1, "да"}
            };

            return (InputCommand(commands, text) == 1);
        }

        static public void ShowBDay(Birthday? bday)
        {
            if (bday != null)
                Console.WriteLine($" {bday.Id,3} | {bday.Date,-11:dd MMMM} | {bday.Person}");
        }

        static public void ShowBDay(int? id, DateTime date, string person)
        {
            if (id == null)
                Console.WriteLine($"     | {date,-11:dd MMMM} | {person}");
            else
                Console.WriteLine($" {id,3} | {date,-11:dd MMMM} | {person}");
        }

        static private string SectionTitle(string title)
        {
            return $"--- {title} ---\n";
        }

        static private string tableHeader = "  ID | Дата        | Имя\n" +
                                            "-------------------------------------------------------";

        static public void ShowAllBDays(ListOfBirthdays bdays)
        {
            Console.WriteLine(SectionTitle("Все дни рождения"));
            Console.WriteLine(tableHeader);

            foreach (var b in bdays)
                ShowBDay(b);
            Console.WriteLine();            
        }

        static public void ShowTodayBDays(ListOfBirthdays bdays)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine(SectionTitle("Сегодняшние дни рождения"));
            Console.WriteLine(tableHeader);

            foreach (var b in bdays)
            {
                DateTime thisYearDate = new(today.Year, b.Date.Month, b.Date.Day);
                if (thisYearDate == today)
                    ShowBDay(b);
            }

            Console.WriteLine();
        }

        static public void ShowNearestBDays(ListOfBirthdays bdays, int nearest = 7, int period = 90)
        {
            Console.WriteLine(SectionTitle("Ближайшие дни рождения"));
            Console.WriteLine(tableHeader);

            ListOfBirthdays bdays_tmp = bdays.SortedByDateFromToday();

            int nearest_counter = 0;
            foreach (var b in bdays_tmp)
                if (nearest_counter < nearest)
                {
                    ShowBDay(b);
                    nearest_counter++;
                }            
            
            Console.WriteLine();
        }

        static public ListOfBirthdays AddBday(ListOfBirthdays bdays)
        {
            Console.WriteLine(SectionTitle("Добавление записи"));

            ListOfBirthdays bdays_new = bdays;

            DateTime? date = InputDate(formatsOfDate);
            if (date != null)
            {
                string? person = InputPerson();
                if (person != null) 
                {
                    Console.WriteLine("\nБудет добавлена запись:");
                    ShowBDay(null, (DateTime)date, person);
                    Console.WriteLine();

                    if (YesNoQuestion("Продолжить?"))
                        bdays_new.Add((DateTime)date, person);
                    else
                        Console.WriteLine("Отменено");
                }
                else
                    Console.WriteLine("Прервано");
            }
            else
                Console.WriteLine("Прервано");
            Console.WriteLine();
            return bdays_new;
        }

        static public ListOfBirthdays DeleteBday(ListOfBirthdays bdays)
        {
            Console.WriteLine(SectionTitle("Удаление записи"));

            ListOfBirthdays bdays_new = bdays;

            int? id = InputId();
            if (id != null)
            {
                if (bdays[(int)id] != null)
                {
                    Console.WriteLine("Будет удалена запись:");
                    ShowBDay(bdays[(int)id]);
                    Console.WriteLine();

                    if (YesNoQuestion("Продолжить?"))
                        bdays_new.Delete((int)id);
                    else
                        Console.WriteLine("Отменено");
                }
                else
                    Console.WriteLine("Запись не найдена.");
            }
            else
                Console.WriteLine("Прервано");
            Console.WriteLine();            
            return bdays_new;
        }

        static public ListOfBirthdays EditBday(ListOfBirthdays bdays)
        {
            Console.WriteLine(SectionTitle("Изменение записи"));

            ListOfBirthdays bdays_new = bdays;

            int? id = InputId();
            if (id != null)
            {
                Birthday? bday = bdays[(int)id];
                if (bday != null) 
                {
                    Console.WriteLine("Будет изменена запись:");
                    ShowBDay(bday);
                    Console.WriteLine();

                    Dictionary<byte, string> cmd_dict = new()
                    {
                        { 0, "дата" },
                        { 1, "имя" }
                    };

                    byte? cmd = InputCommand(cmd_dict, "Какое поле изменить?", true);
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
                                    Console.WriteLine("\nЗапись будет изменена на:");
                                    ShowBDay(id, (DateTime)newDate, bday.Person);
                                }
                                else
                                    Console.WriteLine("Прервано");
                                break;
                            case 1:
                                newPerson = InputPerson();
                                if (newPerson != null)
                                {
                                    Console.WriteLine("\nЗапись будет изменена на:");
                                    ShowBDay(id, bday.Date, newPerson);
                                }
                                else
                                    Console.WriteLine("Прервано");
                                break;
                        }

                        Console.WriteLine();

                        if (YesNoQuestion("Продолжить?"))
                            bdays_new.Edit((int)id, newDate, newPerson);
                        else
                            Console.WriteLine("Отменено");
                    }
                    else
                        Console.WriteLine("Прервано");
                }
                else
                    Console.WriteLine("Запись не найдена.");
            }
            else
                Console.WriteLine("Прервано");
            Console.WriteLine();  
            return bdays_new;
        }
    }
}
