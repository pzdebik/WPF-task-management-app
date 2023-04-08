using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Model;

namespace ToDoApp
{
    public class FileWriter
    {
        public static void WriteToFile(string filename, List<TaskModel> myTasks)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(JsonConvert.SerializeObject(myTasks));
            }
        }
    }
}
