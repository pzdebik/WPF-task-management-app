using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace ClassLibrary.Model
{
    public static class JsonFile
    {
        public static readonly IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        public static void Save(this Calendar calendar, string fileName)
        {
            try
            {
                var serializedObject = JsonConvert.SerializeObject(calendar, Newtonsoft.Json.Formatting.Indented);

                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(serializedObject);
                }

            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy zapisie danych do pliku JSON", exc);
            }

        }

        public static Calendar Read(string fileName)
        {
            try
            {
                string content = null;

                using (StreamReader sr = new StreamReader(fileName))
                {
                    content = sr.ReadToEnd();
                }

                var deserialized = JsonConvert.DeserializeObject<List<TaskModel>>(content);
                Calendar calendar = new Calendar();
                foreach (TaskModel task in deserialized) calendar.AddTask(task);
                return calendar;
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy odczycie danych z pliku JSON", exc);
            }
        }
    }
}