using System.Collections;

namespace pozdravliator
{
    internal class ListOfBirthdays: IEnumerable<Birthday>
    {
        private List<Birthday> bdays = new();

        public int Count { 
            get
            {
                if (bdays?.Count > 0)                
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
            return bdays.GetEnumerator();
        }

        /*public ListOfBirthdays() { }*/

        public ListOfBirthdays(List<Birthday> bdays)
        {
            this.bdays = bdays;
        }

        public void Add(Birthday bday)
        {
            bdays.Add(bday);
        }

        public void Add(DateTime date, string person)
        {
            bdays.Add(new Birthday(null, date, person));
        }

        public void Delete(int id)
        {
            List<Birthday> bdays_new = new();
            foreach (Birthday bday in bdays)            
                if (bday.Id != id)
                    bdays_new.Add(bday);            
            bdays = bdays_new;
        }

        public void Edit(int id, DateTime? date = null, string? person = null)
        {
            Birthday? bday = this[id];
            bday?.Edit(date, person);
        }

        public ListOfBirthdays Copy()
        {
            List<Birthday> bdays_copy = new();
            foreach (Birthday bday in bdays)
                bdays_copy.Add(bday);
            return new ListOfBirthdays(bdays_copy);
        }

        private void SortById()
        {
            bdays.Sort(new BDayComparerById());
        }

        public ListOfBirthdays SortedById()
        {
            ListOfBirthdays bdays_new = this.Copy();
            bdays_new.SortById();
            return bdays_new;
        }

        private void SortByDate()
        {
            bdays.Sort(new BDayComparerByDate());
        }

        public ListOfBirthdays SortedByDate()
        {
            ListOfBirthdays bdays_new = this.Copy();
            bdays_new.SortByDate();
            return bdays_new;
        }

        private void SortByDateFromToday()
        {
            bdays.Sort(new BDayComparerByDateFromToday());
        }

        public ListOfBirthdays SortedByDateFromToday()
        {
            ListOfBirthdays bdays_new = this.Copy();
            bdays_new.SortByDateFromToday();
            return bdays_new;
        }

        private void SortByPerson()
        {
            bdays.Sort(new BDayComparerByPerson());
        }

        public ListOfBirthdays SortedByPerson()
        {
            ListOfBirthdays bdays_new = this.Copy();
            bdays_new.SortByPerson();
            return bdays_new;
        }
    }
}
