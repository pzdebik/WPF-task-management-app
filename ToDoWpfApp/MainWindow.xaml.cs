using ClassLibrary;
using ClassLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using ToDoWpfApp.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace ToDoWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += MainWindow_Closing;
            
        }
        
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!string.IsNullOrEmpty(tbTaskName.Text))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz odrzucić bieżące zadanie?", "Uwaga", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        
        public TaskViewModel TaskViewModelTask { get; set; }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            // data context to obiekt, item.DataContext to link do konkretnego itemu z listy, inaczej obiektu TaskViewModel
            var taskItem = item.DataContext as TaskViewModel;

            TaskViewModelTask = taskItem;
            tbTaskName.Text = $"{TaskViewModelTask.Name}";
            cbPriority.SelectedItem = TaskViewModelTask.TaskPriority;
            dpDate.SelectedDate = TaskViewModelTask.Date;
        }
    }
}
