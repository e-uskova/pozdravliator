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
            List<Birthday> bdays = new()
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

            ListOfBirtdays bdays_list = new(bdays);

            string[] formatsOfDate = { "dd/MM/yyyy" /*, "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy"*/ };

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
                        {
                            DateTime date = Dialogs.InputDate(formatsOfDate);
                            string person = Dialogs.InputPerson();

                            bdays_list.Add(date, person);
                            break;
                        }
                    case 5:
                        {
                            int id = Dialogs.InputId();

                            bdays_list.Delete(id);
                            break;
                        }                        
                    case 6:
                        {
                            int id = Dialogs.InputId();

                            Dictionary<byte, string> cmd_dict = new() 
                            {
                                { 0, "date" }, 
                                { 1, "name" } 
                            };

                            byte cmd = Dialogs.InputCommand(cmd_dict, "Choose field:");

                            switch (cmd)
                            {
                                case 0:
                                    DateTime newDate = Dialogs.InputDate(formatsOfDate);
                                    bdays_list.Edit(id, date:  newDate);
                                    break;
                                case 1:
                                    string newPerson = Dialogs.InputPerson();
                                    bdays_list.Edit(id, person: newPerson);
                                    break;
                                default:
                                    break;
                            }
                            
                            break;                  
                        }
                    default:
                        break;
                
                }

                command = Dialogs.InputCommand(commands_dict);

                Console.Clear();
            }
        }

    }
}