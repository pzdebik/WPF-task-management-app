using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClassLibrary.Model;

namespace ToDoWpfApp.ViewModel
{
    public class TaskViewModel : ObservedObjects
    {
        private TaskModel model;

        #region Properties

        public Guid Id
        {
            get { return model.Id; }
        }

        public string Name
        {
            get { return model.Name; }
        }

        public Priority TaskPriority
        {
            get { return model.TaskPriority; }
        }

        public DateTime Date
        {
            get
            { return model.Date; }
        }

        public bool IsCompleted
        {
            get { return model.IsCompleted; }
        }

        // zwraca true, jeśli zadanie jest przeterminowane, przekroczyło termin realizacji
        public bool IsOverdue
        {
            get { return !IsCompleted && (DateTime.Now > Date); }
        }
        #endregion

        public TaskViewModel(TaskModel model)
        {
            this.model = model;
        }

        public TaskViewModel(Guid id, string name, DateTime date, Priority taskPriority, bool isCompleted)
        {
            model = new TaskModel(id, name, date, taskPriority, isCompleted); 
        }

        // zwraca TaskModel
        public TaskModel GetModel()
        {
            return model;
        }

        #region Commands
        private ICommand setCompleted;

        public ICommand SetCompleted
        {
            get 
            { 
                if (setCompleted == null) setCompleted = 
                        new RelayCommand(
                        o =>
                        {
                            model.IsCompleted = true;
                            onPropertyChanged(nameof(IsCompleted), nameof(IsOverdue));
                        },
                        o =>
                        {
                            return !model.IsCompleted;
                        });
                return setCompleted;
            }
        }

        private ICommand setNotCompleted;

        public ICommand SetNotCompleted
        {
            get
            {
                if (setNotCompleted == null) setNotCompleted =
                        new RelayCommand(
                        o =>
                        {
                            model.IsCompleted = false;
                            onPropertyChanged(nameof(IsCompleted), nameof(IsOverdue));
                        },
                        o =>
                        {
                            return model.IsCompleted;
                        });
                return setNotCompleted;
            }
        }
        #endregion
        // pokazuje nazwę, datę, priorytet.. zadania, a nie nazwę klasy, która te dane przechowuje
        public override string ToString()
        {
            return model.ToString();
        }
    }
}
