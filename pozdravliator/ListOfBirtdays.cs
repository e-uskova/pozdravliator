using System.Collections;

namespace pozdravliator
{
    internal class ListOfBirtdays: IEnumerable<Birthday>
    {
        private List<Birthday> bdays = new();

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
            bday?.Edit(date, person);
        }
    }
}
