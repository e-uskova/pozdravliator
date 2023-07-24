using System.Globalization;

namespace pozdravliator
{
    static internal class FileIO
    {
        static public void WriteTxt(ListOfBirtdays bdays, string filename = "bdays.txt")
        {
            try
            {
                StreamWriter sw = new(filename);
                foreach (Birthday bday in bdays)
                {
                    sw.WriteLine($"{bday.Id},{bday.Date:dd/MM/yyyy},{bday.Person}");
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        static public ListOfBirtdays ReadTxt(string filename = "bdays.txt")
        {
            ListOfBirtdays bdays = new();

            string? line;
            try
            {
                StreamReader sr = new(filename);
                line = sr.ReadLine();
                Birthday? bday;
                while (line != null)
                {
                    bday = ReadBday(line);
                    if (bday != null)
                        bdays.Add(bday);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return bdays;
        }

        static private Birthday? ReadBday(string? line)
        {
            if (line == null)
            {
                Console.WriteLine("Error during loading bday. Data line is null");
                return null;
            }

            string[] data = line.Split(',');
            if (data.Length < 3)
            {
                Console.WriteLine("Error during loading bday. Wrong data line length");
                return null;
            }

            int id;
            try
            {
                id = Convert.ToInt32(data[0]);
            }
            catch
            {
                Console.WriteLine("Error during loading bday. Wrong id " + data[0]);
                return null;
            }

            if (!DateTime.TryParseExact(data[1], "dd/MM/yyyy", new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime date))
            {
                Console.Write("Error during loading bday. Wrong date " + data[0]);
                return null;
            }

            string person = data[2];

            return new Birthday(id, date, person);
        }
    }
}
