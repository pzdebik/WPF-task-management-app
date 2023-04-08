

namespace ClassLibrary.Model
{
    public enum Priority : byte { Niski, Średni, Wysoki }
    public class TaskModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public string? Name { get; set; }

        public Priority TaskPriority { get; set; }

        public bool IsCompleted { get; set; }

        public TaskModel(Guid id, string name, DateTime date, Priority taskPriority, bool isCompleted = false)
        {
            this.Id = id;
            this.Name = name;
            this.Date = date;
            this.TaskPriority = taskPriority;
            this.IsCompleted = isCompleted;
        }

        public override string ToString()
        {
            return Id.ToString() + Name + ", priorytet: " + TaskPriority.ToString()
                + ", planowany termin realizacji: " + Date.ToString()
                + ", czy zrealizowane?: " + (IsCompleted ? "Tak" : "Nie");
        }

    }
}