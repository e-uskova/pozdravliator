using System.IO;

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
            string filename = "Scientists.txt"; /*"Birthdays.txt";*/
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var path = Path.Combine(projectDirectory, filename);

            var json = File.ReadAllText(path);
            List<Birthday> bdays = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Birthday>>(json) ?? new();
            ListOfBirthdays bdays_list = new(bdays);

            /*ListOfBirthdays bdays_list = new()
            {
                new Birthday(new DateTime(2000, 5, 2), "Kate"),
                new Birthday(new DateTime(1998, 7, 17), "Lana"),
                new Birthday(new DateTime(1997, 1, 29), "Alex"),
                new Birthday(new DateTime(2003, 5, 15), "Rina"),
                new Birthday(new DateTime(2022, 9, 6), "Shkiper"),
                new Birthday(new DateTime(2023, 7, 4), "This code"),
                new Birthday(DateTime.Today, "Today"),
                new Birthday(DateTime.Today.AddYears(-5), "Today 5 years ago")
            };
            
            var jsonOut = Newtonsoft.Json.JsonConvert.SerializeObject(bdays_list);
            File.WriteAllText(path, jsonOut);*/

            Dictionary<byte, string> commands_dict = new()
            {
                { 0, "выход" },
                { 1, "показать все дни рождения" },
                { 10, "сортировать по ID" },
                { 11, "сортировать по дате" },
                { 12, "сортировать по имени" },
                { 2, "показать ближайшие дни рождения" },
                { 3, "добавить запись" },
                { 4, "удалить запись" },
                { 5, "изменить запись" }
            };

            Console.WriteLine("*** ПОЗДРАВЛЯТОР ***\n");

            byte? command = 1;

            while (command != 0)
            {
                switch (command)
                {
                    case 1:
                        Dialogs.ShowAllBDays(bdays_list);
                        break;
                    case 10:
                        Dialogs.ShowAllBDays(bdays_list.SortedById());
                        break;
                    case 11:
                        Dialogs.ShowAllBDays(bdays_list.SortedByDate());
                        break;
                    case 12:
                        Dialogs.ShowAllBDays(bdays_list.SortedByPerson());
                        break;
                    case 2:
                        Dialogs.ShowNearestBDays(bdays_list);
                        break;
                    case 3:
                        bdays_list = Dialogs.AddBday(bdays_list);
                        break;
                    case 4:
                        bdays_list = Dialogs.DeleteBday(bdays_list);
                        break;
                    case 5:
                        bdays_list = Dialogs.EditBday(bdays_list);
                        break;
                    default: break;
                }

                command = Dialogs.InputCommand(commands_dict);

                if (command != 3 && command != 4 && command != 5 && command != 0)
                    Console.Clear();
                else 
                    Console.WriteLine();
            }

            if (Dialogs.YesNoQuestion("Сохранить изменения?"))
            {
                var jsonOut = Newtonsoft.Json.JsonConvert.SerializeObject(bdays_list);
                File.WriteAllText(path, jsonOut);
            }
        }

    }
}