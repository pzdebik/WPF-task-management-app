using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoWpfApp.ViewModel;
using ClassLibrary.Model;

namespace ToDoWpfApp
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        public EditTask()
        {
            InitializeComponent();
        }
        public TaskViewModel TaskViewModelTask { get; set; }
        public void ShowTask(TaskViewModel taskItem)
        {
            TaskViewModelTask = taskItem;
            tbNewTaskName.Text = $"{TaskViewModelTask.Name}";
            cbNewPriority.SelectedItem = TaskViewModelTask.TaskPriority;
            dpNewDate.SelectedDate = TaskViewModelTask.Date;
            Show();
        }
    }
}
