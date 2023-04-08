using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ClassLibrary.Model;
using static ClassLibrary.Model.JsonFile;

namespace ToDoWpfApp.ViewModel
{
    public class TaskManager
    {
        private Calendar model;
        public ObservableCollection<TaskViewModel> TaskList { get;} = new ObservableCollection<TaskViewModel>();
        public string NewTaskName { get; set; }
        public List<Priority> PriorityCollection { get; set; } = new List<Priority> { Priority.Niski, Priority.Średni, Priority.Wysoki };
        public Priority NewPriority { get; set; }
        public bool IsChanged = false;
        public ObservableCollection<SortOption> SortOptions { get; } = new ObservableCollection<SortOption>
        {
            new SortOption {Name = "Po dacie rosnąco", CommandParameter = "DateAsc"},
            new SortOption {Name = "Po dacie malejąco", CommandParameter = "DateDesc"},
            new SortOption {Name = "Po priorytecie rosnąco", CommandParameter = "PriorityAsc"},
            new SortOption {Name = "Po priorytecie malejąco", CommandParameter = "PriorityDesc"}
        };



        private void copyFromModel()
        {
            TaskList.CollectionChanged -= synchornizeModel; // po to, aby synchronizacja obustronna między modelem a viewmodelem
                                                            // została wyłączona przed wyczyczeniem listy zadań w observable collection TaskList.
                                                            // Jeśli metoda synchronizująca byłaby podłączona, usunęła by także zadania z modelu
            TaskList.Clear();
            foreach (TaskModel task in model)
                TaskList.Add(new TaskViewModel(task));
            TaskList.CollectionChanged += synchornizeModel;
        }
        private string fileName = "zadania.json";

        // Kopiowanie zadań z modelu do view modelu
        
        public TaskManager()
        {
            if (System.IO.File.Exists(fileName)) model = Read(fileName);
            else model = new Calendar();

            copyFromModel();
        }
        

        // synchronizacja działań po stronie modelu i viewmodelu; jeśli usuniemy w jednym, drugi zostanie zaktualizowany
        public void synchornizeModel(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TaskViewModel newTask = (TaskViewModel)e.NewItems[0];
                    if (newTask != null) model.AddTask(newTask.GetModel());
                    IsChanged = true;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TaskViewModel deletedTask = (TaskViewModel)e.OldItems[0];
                    if (deletedTask != null) model.RemoveTask(deletedTask.GetModel());
                    IsChanged = true;
                    break;
            }
        }

        public ICommand Save
        {
            get
            {
                return new RelayCommand(
                     argument =>
                     {
                         if (IsChanged)
                         {
                             MessageBoxResult result = MessageBox.Show("Czy zapisać zmiany?", "Uwaga", MessageBoxButton.YesNo);
                             if (result == MessageBoxResult.Yes)
                             {
                                 JsonFile.Save(model, fileName);
                             }
                         } 

                     });
            }
        }

        private int taskIndex;
        public int TaskIndex
        {
            get { return taskIndex; }
            set { taskIndex = value; OnPropertyChanged("TaskIndex"); }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged("StartDate"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        private ICommand removeTask;
        public ICommand RemoveTask
        {
            get
            {
                if (removeTask == null) removeTask = new RelayCommand(
                    o =>
                    {
                        int taskIndex = (int)o;
                        TaskViewModel task = TaskList[taskIndex];
                        TaskList.Remove(task); // dzięki synchronizacji powinno usunąc także w Task Modelu
                    },
                    o =>
                    {
                        if (o == null) return false;
                        int taskIndex = (int)o;
                        return taskIndex >= 0;
                    });
                return removeTask;
            }
        }

        private ICommand addTask;
        public ICommand AddTask
        {
            get
            {
                if (addTask == null) addTask = new RelayCommand(
                    o =>
                    {
                        TaskViewModel task = o as TaskViewModel;
                        if (task != null)
                        {
                            TaskList.Add(task);
                        }
                    },
                    o =>
                    {
                        return (o as TaskViewModel) != null;
                    });
                return addTask;
            }
        }

        private SortOption selectedSortOption;
        public SortOption SelectedSortOption
        {
            get { return selectedSortOption; }
            set
            {
                if (selectedSortOption != value)
                {
                    selectedSortOption = value;

                    SortCommand.Execute(selectedSortOption.CommandParameter);
                }
            }
        }

        private ICommand sortCommand;
        public ICommand SortCommand
        {
            get
            {
                if (sortCommand == null)
                {
                    sortCommand = new RelayCommand(
                        o =>
                        {
                            string sortType = o as string;
                            if (sortType != null)
                            {
                                switch (sortType)
                                {
                                    case "DateAsc":
                                        model.SortByDateAscending();
                                        copyFromModel();
                                        IsChanged = true;
                                        break;
                                    case "DateDesc":
                                        model.SortByDateDescending();
                                        copyFromModel();
                                        IsChanged = true;
                                        break;
                                    case "PriorityAsc":
                                        model.SortByPriorityAscending();
                                        copyFromModel();
                                        IsChanged = true;
                                        break;
                                    case "PriorityDesc":
                                        model.SortByPriorityDescending();
                                        copyFromModel();
                                        IsChanged = true;
                                        break;
                                }
                            }
                        },
                        o => (o as string) != null
                        );
                }
                return sortCommand;
            }
        }

        

        
        private ICommand updateTask;
        public ICommand UpdateTask
        {
            get
            {
                if (updateTask == null) updateTask = new RelayCommand(
                o =>
                {
                    
                    int taskIndex = (int)o;
                    TaskViewModel task = TaskList[taskIndex];

                    if (!string.IsNullOrEmpty(NewTaskName))
                    {
                        task.GetModel().Name = NewTaskName;
                    }

                    task.GetModel().Date = StartDate;

                    
                    task.GetModel().TaskPriority = NewPriority;
                    copyFromModel();
                    IsChanged = true;
                },
                o =>
                {
                    if (o == null) return false;
                    int taskIndex = (int)o;
                    return taskIndex >= 0;
                });
                return updateTask;
            }
        }

        // komenda wywoływana po podwójnym kliknięciu na taska (gdy w xamlu użyjemy <i:Interaction.Triggers>)
        // otwiera nowe okno z danymi klikniętego taska
        // obecnie nie używana zastąpiona podówjnym kliknięciem na taska i pojawieniem się danych w tym samym oknie
        /*
        private ICommand showTask;
        public ICommand ShowTask
        {
            get
            {
                if (showTask == null) showTask = new RelayCommand(
                o =>
                {
                    TaskIndex = (int)o;
                    TaskViewModel task = TaskList[TaskIndex];
                    //TaskViewModel task = o as TaskViewModel;
                    EditTask secondWindow = new EditTask();
                    secondWindow.ShowTask(task);
                    copyFromModel();
                },
                o =>
                {
                    return true;
                });
                return showTask;
            }
        }
        */

        // komenda ma za zadanie nadpisać klikniętego taska, po wciśnięciu buttona Zapisz jednak z uwagi na to, że konstruktor TaskManger
        // wywołuje się po raz drugi przy otwarciu nowego okna, tworząc nową instancję, dane nadpisywane są dopiero po zamknięciu apki
        // obecnie nie używana, do naprawienia
        private ICommand updateDoubleClickedTask;
        public ICommand UpdateDoubleClickedTask
        {
            get
            {
                if (updateDoubleClickedTask == null) updateDoubleClickedTask = new RelayCommand(
                o =>
                {
                    TaskViewModel task = TaskList[0];
                    task.GetModel().Name = NewTaskName;
                    task.GetModel().Date = StartDate;
                    task.GetModel().TaskPriority = NewPriority;
                    JsonFile.Save(model, fileName);
                    copyFromModel();
                    IsChanged = true;
                },
                o =>
                {
                    return true;
                });
                return updateDoubleClickedTask;
            }
        }
    }
}
