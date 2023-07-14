using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;


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
            string filename = "Birthdays.txt";

            /*ListOfBirtdays bdays_list = new()
            {
                new Birthday(new DateTime(2000, 5, 2), "Kate"),
                new Birthday(new DateTime(1998, 7, 17), "Lana"),
                new Birthday(new DateTime(1997, 1, 29), "Alex"),
                new Birthday(new DateTime(2003, 5, 15), "Rina"),
                new Birthday(new DateTime(2022, 9, 6), "Shkiper"),
                new Birthday(new DateTime(2023, 7, 4), "This code"),
                new Birthday(DateTime.Today, "Today"),
                new Birthday(DateTime.Today.AddYears(-5), "Today 5 year ago")
            };

            FileIO.WriteTxt(bdays_list, filename);*/

            ListOfBirtdays bdays_list = FileIO.ReadTxt(filename);

            Dictionary<byte, string> commands_dict = new()
            {
                { 0, "exit" },
                { 1, "show all bdays" },
                { 2, "today bdays" },
                { 3, "nearest bdays" },
                { 4, "add bday" },
                { 5, "delete bday" },
                { 6, "edit bday" }
            };

            Console.WriteLine("*** Welcome to Pozdravliator! ***\n");

            byte command = 1;

            while (command != 0)
            {
                switch (command)
                {
                    case 1:
                        bdays_list.ShowAllBDays();
                        break;
                    case 2:
                        bdays_list.ShowTodayBDays();
                        break;
                    case 3:
                        bdays_list.ShowNearestBDays();
                        break;
                    case 4:
                        bdays_list = Dialogs.AddBday(bdays_list);
                        break;
                    case 5:
                        bdays_list = Dialogs.DeleteBday(bdays_list);
                        break;
                    case 6:
                        bdays_list = Dialogs.EditBday(bdays_list);
                        break;
                    default:
                        break;                
                }

                command = Dialogs.InputCommand(commands_dict);

                Console.Clear();
            }

            if (Dialogs.YesNoQuestion("Save changes?"))
                FileIO.WriteTxt(bdays_list, filename);
        }

    }
}