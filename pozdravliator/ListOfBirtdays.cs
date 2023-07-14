using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pozdravliator
{
    internal class ListOfBirtdays: IEnumerable<Birthday>
    {
        private List<Birthday> bdays = new List<Birthday>();

        public int Count { 
            get
            {
                if (bdays.Count > 0)                
                    return bdays.Count;                
                else  
                    return 0;                 
            } 
        }

        public Birthday? this[int index_]
        {
            get
            {
                int index = index_;
                if (index < 0 && -index < bdays.Count)
                    index = bdays.Count - index;

                foreach (Birthday bday in bdays)
                    if (bday.Id == index)
                        return bday;
                return null;
            }
        }

        public IEnumerator<Birthday> GetEnumerator()
        {
            return bdays.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public ListOfBirtdays() { }

        public ListOfBirtdays(List<Birthday> bdays)
        {
            this.bdays = bdays;
        }

        public void Add(Birthday bday)
        {
            bdays.Add(bday);
        }

        public void Add(DateTime date, string person)
        {
            bdays.Add(new Birthday(date, person));
        }

        public void Delete(int id)
        {
            List<Birthday> bdays_new = new();
            foreach (Birthday bday in bdays)
            {
                if (bday.Id != id)
                    bdays_new.Add(bday);
            }
            bdays = bdays_new;
        }

        public void Edit(int id, DateTime? date = null, string? person = null)
        {
            Birthday? bday = this[id];
            if (bday == null)
                Console.WriteLine("No such record");
            else
                bday.Edit(date, person);
        }

        /*public void ShowBDay(int id)
        {
            Birthday? bday = this[id];
            if (bday == null)
                Console.WriteLine("No such record");
            else
                Console.WriteLine($" {bday.Id,3} | {bday.Date,-8:dd/MM/yy} | {bday.Person}");
        }*/

        public void ShowBDay(Birthday? bday)
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

        public void ShowAllBDays()
        {
            Console.WriteLine("--- All Birthdays ---\n");
            Console.WriteLine("  ID | Date     | Person\n" +
                              "----------------------------");
            foreach (var b in bdays)
                ShowBDay(b);
            Console.WriteLine();
        }

        public void ShowTodayBDays()
        {
            DateTime today = DateTime.Today;

            Console.WriteLine("--- Today Birthdays ---\n");
            Console.WriteLine("  ID | Date     | Person\n" +
                              "----------------------------");
            foreach (var b in bdays)
            {
                DateTime thisYearDate = new(today.Year, b.Date.Month, b.Date.Day);
                if (thisYearDate == today)
                    ShowBDay(b);
            }

            Console.WriteLine();
        }

        public void ShowNearestBDays(ushort period = 90)
        {
            DateTime today = DateTime.Today;

            Console.WriteLine("--- Nearest Birthdays ---\n");
            Console.WriteLine("  ID | Date     | Person\n" +
                              "----------------------------");

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

    }
}
