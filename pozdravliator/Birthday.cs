using System;

namespace pozdravliator
{
    internal class Birthday
    {
        static ushort counter = 0;

        private ushort id;
        private DateTime date;
        private string person;

        public Birthday(DateTime date, string person)
        {
            id = counter++;
            this.date = date;
            this.person = person;
        }

        public DateTime Date { get { return date; } }
        public string Person { get { return person; } }
        public ushort Id { get { return id; } }

        public void Edit(DateTime? date = null, string? person = null)
        {
            if (date != null) 
                this.date = (DateTime)date;
            if (person != null)
                this.person = person;
        }
    }
}
