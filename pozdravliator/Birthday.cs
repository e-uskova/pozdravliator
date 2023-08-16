using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace pozdravliator
{   
    class BDayComparerById : IComparer<Birthday>
    {
        public int Compare(Birthday? b1, Birthday? b2)
        {
            if (b1 != null && b2 != null)
                return b1.Id.CompareTo(b2.Id);
            else
                return 0;
        }
    }

    class BDayComparerByDate : IComparer<Birthday>
    { 
        public int Compare(Birthday? b1, Birthday? b2)
        {
            if (b1 != null && b2 != null)
            {
                DateTime sameYearDate1 = new(1, b1.Date.Month, b1.Date.Day);
                DateTime sameYearDate2 = new(1, b2.Date.Month, b2.Date.Day);

                return DateTime.Compare(sameYearDate1, sameYearDate2);
            }
            else
                return 0;
        }
    }

    class BDayComparerByDateFromToday : IComparer<Birthday>
    {
        private DateTime futureDate(DateTime date)
        {
            DateTime today = DateTime.Today;

            DateTime thisYearDate = new(today.Year, date.Month, date.Day);
            if (thisYearDate < today)
                return thisYearDate.AddYears(1);
            else
                return thisYearDate;
        }

        public int Compare(Birthday? b1, Birthday? b2)
        {
            if (b1 != null && b2 != null)
            {
                DateTime futureDate1 = futureDate(b1.Date);
                DateTime futureDate2 = futureDate(b2.Date);

                return DateTime.Compare(futureDate1, futureDate2);
            }
            else
                return 0;
        }
    }

    class BDayComparerByPerson : IComparer<Birthday>
    {
        public int Compare(Birthday? b1, Birthday? b2)
        {
            return String.Compare(b1?.Person, b2?.Person);
        }
    }

    internal class Birthday 
    {
        static private int counter = 0;

        private int id;
        private DateTime date;
        private string person;

        public Birthday(int? id, DateTime date, string person)
        {
            if (id == null)
                id = counter++;
            else
            {
                if (id < counter)
                    Console.WriteLine($"Id {id} >= record counter {counter}. Overwriting may occur.");
                else
                    counter = (int)id + 1;
            }

            this.id = (int)id;
            this.date = date;
            this.person = person;
        }

        public DateTime Date { get { return date; } set { } }
        public string Person { get { return person; } set { } }
        public int Id { get { return id; } set { } }

        public void Edit(DateTime? date = null, string? person = null)
        {
            if (date != null) 
                this.date = (DateTime)date;
            if (person != null)
                this.person = person;
        }

        public int CompareTo(Birthday? other)
        {
            if (other != null)
            {
                int this_day = this.Date.DayOfYear;
                int other_day = other.Date.DayOfYear;
                int diff = other_day - this_day;
                if (diff < 0 || diff >= 183)
                    return -1;
                else if (diff > 0 && diff < 183)
                    return 1;
                else /* diff == 0 */
                    return 0;
            }
            else
                return -1;
        }
    }
}
