using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassLibrary.Model
{
    public class Calendar : IEnumerable<TaskModel>
    {
        public string filename = "TaskList.json";

        public List<TaskModel> myTasks = new List<TaskModel>();

        public void AddTask(TaskModel task)
        {
            myTasks.Add(task);
        }

        public void RemoveTask(TaskModel task)
        {
            myTasks.Remove(task);
        }

        public List<TaskModel> HideDoneTasks(List<TaskModel> tempList, TaskModel task)
        {
            tempList.Add(task);
            return tempList;
        }

        public void ChangePriority(TaskModel task, Priority priority)
        {
            // zmienia stan właściwości PriorityLevel
            task.TaskPriority = priority;
        }

        public void MoveTask(TaskModel task, DateTime date)
        {
            task.Date = date;
        }

        public void SortByDateAscending()
        {
            myTasks.Sort((x, y) => x.Date.CompareTo(y.Date));

        }

        public void SortByDateDescending()
        {
            myTasks.Sort((x, y) => y.Date.CompareTo(x.Date));
        }

        public void SortByPriorityAscending()
        {
            myTasks.Sort((x, y) => x.TaskPriority.CompareTo(y.TaskPriority));
        }

        public void SortByPriorityDescending()
        {
            myTasks.Sort((x, y) => y.TaskPriority.CompareTo(x.TaskPriority));
        }

        public void EndTask(TaskModel task, bool status)
        {
            task.IsCompleted = status;
        }

        public IEnumerator<TaskModel> GetEnumerator()
        {
            return myTasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public int TasksCount
        {
            get
            {
                return myTasks.Count;
            }
        }

        public TaskModel this[int index] //indexer
        {
            get
            {
                return myTasks[index];
            }
        }

    }
}
