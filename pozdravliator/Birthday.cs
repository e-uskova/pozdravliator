using System;

namespace pozdravliator
{
    internal class Birthday
    {
        static int counter = 0;

        private int id;
        private DateTime date;
        private string person;

        public Birthday(int id, DateTime date, string person)
        {
            if (id < counter)
                Console.WriteLine($"Id {id} >= record counter {counter}. Overwriting may occur.");
            else
                counter = id + 1;

            this.id = id;
            this.date = date;
            this.person = person;
        }

        public Birthday(DateTime date, string person)
        {
            id = counter++;
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
    }
}
