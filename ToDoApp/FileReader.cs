using Newtonsoft.Json;
using ClassLibrary.Model;

namespace ToDoApp
{
    public class FileReader
    {
        public static List<TaskModel> ReadFile(string filename)
        {
            List<TaskModel> myNewTasks = new List<TaskModel>();
            if (File.Exists(filename))
            {
                string readJson = File.ReadAllText(filename);
                myNewTasks = JsonConvert.DeserializeObject<List<TaskModel>>(readJson);
            }
            return myNewTasks;
        }
    }
}
