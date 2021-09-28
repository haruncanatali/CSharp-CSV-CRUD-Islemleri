using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace CsvCalisma
{
    class Program
    {
        static string connectionString = @"C:\Users\haruncan.atali\Desktop\deneme_db.csv";
        static string sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + connectionString + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
        static string line = String.Empty;
        static string[] split;
        static List<string> lines;
        static Random random = new Random();

        static void Main(string[] args)
        {
            Insert(random.Next(1, 10000), "Ekrem", "Kemal");
            Read();
            Console.Read();
        }

        static void Read()
        {
            List<User> userList = new List<User>();

            using (var reader = new StreamReader(connectionString))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    userList = csvReader.GetRecords<User>().ToList();
                }
                reader.Close();
            }

            foreach (var item in userList)
            {
                Console.WriteLine($"Id:{item.ID} Ad:{item.AD} Soyad:{item.SOYAD}");
            }
        }

        static void BulkInsert(List<User> modelList)
        {
            using (var writer = new StreamWriter(connectionString, false, new UTF8Encoding(true)))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<User>();
                    csvWriter.NextRecord();
                    foreach (var item in modelList)
                    {
                        csvWriter.WriteRecord<User>(item);
                        csvWriter.NextRecord();
                    }
                }
                writer.Close();
            }
        }

        static void Insert(int id, string ad, string soyad)
        {
            string temp = id + "," + ad + "," + soyad;
            using (StreamWriter writer = new StreamWriter(connectionString,true,Encoding.UTF8))
            {
                writer.WriteLine(temp);
                writer.Close();
            }
        }

        static void Update(int id, string ad, string soyad)
        {
            lines = new List<string>();

            using (StreamReader reader = new StreamReader(connectionString))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    split = line.Split(',');
                    if (split[0] == id.ToString())
                    {
                        split[1] = ad;
                        split[2] = soyad;
                        line = String.Join(",", split);
                    }
                    lines.Add(line);
                }
                reader.Close();
            }

            using (StreamWriter writer = new StreamWriter(connectionString, false, Encoding.UTF8))
            {
                foreach (var item in lines)
                {
                    writer.WriteLine(item);
                }
                writer.Close();
            }
        }

        static void Delete(int id)
        {
            lines = new List<string>();

            using (StreamReader reader = new StreamReader(connectionString))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    split = line.Split(',');
                    lines.Add(String.Join(",", split));
                }
                reader.Close();
            }

            int index=-1;

            for (int i = 0; i < lines.Count; i++)
            {
                split = lines[i].Split(',');
                if (split[0] == $"\"{id.ToString()}\"")
                {
                    index = i;
                }
            }

            if (index!=-1)
            {
                lines.RemoveAt(index);
            }

            string temp = String.Empty;
            for (int i = 0; i < lines.Count; i++)
            {
                temp = lines[i].Replace("\"","");
                lines[i] = temp;
            }

            using (StreamWriter writer = new StreamWriter(connectionString, false, Encoding.UTF8))
            {
                foreach (var item in lines)
                {
                    writer.WriteLine(item);
                }
                writer.Close();
            }
        }
    }
}
