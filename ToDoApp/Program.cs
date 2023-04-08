using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using ClassLibrary.Model;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using System.Diagnostics;

namespace ToDoApp
{
    public class Program
    {
        public static string filename = "TaskList.json";
        static void Main()
        {
            Calendar calendar = new Calendar();
            calendar.myTasks = FileReader.ReadFile(filename);
            List<TaskModel> list = calendar.myTasks;
            Console.WriteLine("Wciśnij ENTER, aby wyświetlić MENU.");
            ConsoleKey key = Console.ReadKey().Key;


            while (key != ConsoleKey.E)
            {
                Console.Clear();
                Console.WriteLine("Co chciałbyś zrobić?");
                Console.WriteLine("1. Wpisz 1, aby dodać zadanie do listy.");
                Console.WriteLine("2. Wpisz 2, aby usunąć zadanie z listy.");
                Console.WriteLine("3. Wpisz 3, aby zobaczyć listę zadań.");
                Console.WriteLine("4. Wpisz 4, aby zmienić priorytet zadania.");
                Console.WriteLine("5. Wpisz 5, aby przenieść zadanie na inny dzień.");
                Console.WriteLine("6. Wpisz 6, aby posortować zadania");
                Console.WriteLine("7. Wpisz 7, aby zakończyć zadanie");
                Console.WriteLine("8. Wpisz e, aby wyjść z programu.");

                key = Console.ReadKey().Key;

                // opcja 1.
                if (key == ConsoleKey.D1)
                {
                    Console.Clear();

                    var newTask = new TaskModel(Guid.NewGuid(), GetTask(), GetDate(), GetPriority(), false);
                    
                    calendar.AddTask(newTask);

                    Console.Clear();
                    Console.WriteLine("Zadanie zostało dodane do listy");
                    Console.WriteLine("Naciśnij dowolny przycisk, aby przejść do MENU");
                    Console.ReadKey();

                }
                // opcja 2. - wyświetla wszystkie zadania w aplikacji, a po wpisaniu przypisanego do niego numeru usuwa je
                else if (key == ConsoleKey.D2)
                {
                    Console.Clear();
                    // show task
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (var task in list)
                        {
                            Console.WriteLine($"{++i}. {task.Date.ToShortDateString()}: {task.Name} " +
                                $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                        }
                    }

                    Console.WriteLine("Wpisz numer zadania, które chcesz usunąć: ");
                    int index = Int32.Parse(Console.ReadLine());
                    TaskModel chosenTask = list.ElementAt(index - 1);

                    while (index >= 0)
                    {
                        if (index < 1 || index > list.Count)
                        {
                            Console.WriteLine("Nie ma takiego zadania. Wybierz numer zadania z listy: ");
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (var task in list)
                                {
                                    Console.WriteLine($"{++i}. {task.Date}: {task.Name} " +
                                        $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                                }
                            }
                            index = int.Parse(Console.ReadLine());
                        }
                        else
                        {
                            calendar.RemoveTask(chosenTask);
                            break;
                        }
                    }       
                }
                // opcja 3. - pokazanie wszystkich zadań
                else if (key == ConsoleKey.D3)
                {
                    Console.Clear();
                    if (list.Count == 0)
                    {
                        Console.WriteLine("Brak zadań na liście");
                        Console.WriteLine("Wciśnij dowolny przycisk, aby wyjść do MENU");
                        Console.ReadKey();
                    }
                    else
                    {
                        // show task
                        for (int i = 0; i < list.Count; i++)
                        {
                            foreach (var task in list)
                            {
                                Console.WriteLine($"{++i}. {task.Date.ToShortDateString()}: {task.Name} " +
                                    $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                            }
                        }
                        Console.WriteLine("Chcesz ukryć zrobione zadania? (t/n)");

                        key = Console.ReadKey().Key;
                        if (key == ConsoleKey.T)
                        {
                            Console.Clear();
                            List<TaskModel> tempList = new List<TaskModel>();
                            int counter = 0;
                            foreach (var task in list)
                            {
                                if (task.IsCompleted == false)
                                {
                                    Console.WriteLine($"{++counter}. {task.Date.ToShortDateString()}: {task.Name} " +
                                                      $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                                }
                                else if (task.IsCompleted == true)
                                {
                                   tempList = calendar.HideDoneTasks(tempList, task);
                                }
                            }
                            if (tempList.Count == list.Count) // było: calendar.HideDoneTasks()
                            {
                                Console.WriteLine("Brak zadań na liście. Wszystko zrobione!");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("Oto Twoje wszystkie niewykonane zadania.");
                                Console.WriteLine("Wciśnij dowolny przycisk, aby wyjść do MENU");
                                Console.ReadKey();
                            }
                        }
                        else if (key == ConsoleKey.N)
                        {
                            continue;
                        }
                    }
                    
                }
                // opcja 4. - zmiana priorytetu zadania
                else if (key == ConsoleKey.D4)
                {
                    Console.Clear();
                    // show task
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (var task in list)
                        {
                            Console.WriteLine($"{++i}. {task.Date.ToShortDateString()}: {task.Name} " +
                                $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                        }
                    }
                    Console.WriteLine("Wpisz numer zadania, którego priorytet chcesz zmienić: ");

                    int index = Int32.Parse(Console.ReadLine());
                    while (index >= 0)
                    {
                        if (index < 1 || index > list.Count)
                        {
                            Console.WriteLine("Nie ma takiego zadania. Wybierz numer zadania z listy: ");
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (var task in list)
                                {
                                    Console.WriteLine($"{++i}. {task.Date}: {task.Name} " +
                                        $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                                }
                            }
                            index = int.Parse(Console.ReadLine());
                        }
                        else
                        {
                            calendar.ChangePriority(list.ElementAt(index - 1), GetPriority());
                            break;
                        }
                    }
                }
                // opcja 5. - przeniesienie zadania na inną datę
                else if (key == ConsoleKey.D5)
                {
                    Console.Clear();
                    // show task
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (var task in list)
                        {
                            Console.WriteLine($"{++i}. {task.Date.ToShortDateString()}: {task.Name} " +
                                $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                        }
                    }

                    Console.WriteLine("Wpisz numer zadania, które chcesz przenieść na inny dzień: ");
                    int index = Int32.Parse(Console.ReadLine());


                    while (index >= 0)
                    {

                        if (index < 1 || index > list.Count)
                        {
                            Console.WriteLine("Nie ma takiego zadania. Wybierz numer zadania z listy: ");
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (var task in list)
                                {
                                    Console.WriteLine($"{++i}. {task.Date}: {task.Name} " +
                                        $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                                }
                            }
                            index = int.Parse(Console.ReadLine());
                        }
                        else
                        {
                            // wyciąga wybrany przez użytkownika element z listy
                            
                            calendar.MoveTask(list.ElementAt(index - 1), GetDate());
                            break;
                        }
                    }
                }
                // opcja 6. - sortowanie 
                else if (key == ConsoleKey.D6)
                {
                    Console.Clear();
                    Console.WriteLine("Jak chcesz posortować?");
                    Console.WriteLine("1. Po dacie");
                    Console.WriteLine("2. Po priorytecie");
                    ConsoleKey choose = Console.ReadKey().Key;

                    if (choose == ConsoleKey.D1)
                    {
                        Console.Clear();
                        Console.WriteLine("Malejąco, czy rosnąco?");
                        Console.WriteLine("1. Rosnąco");
                        Console.WriteLine("2. Malejąco");
                        choose = Console.ReadKey().Key;

                        if (choose == ConsoleKey.D1)
                        {
                            Console.Clear();
                            calendar.SortByDateAscending();
                        }
                        else if (choose == ConsoleKey.D2)
                        {
                            Console.Clear();
                            calendar.SortByDateDescending();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Nieprawidłowy numer. Wybierz między 1, a 2.");
                            continue;
                        }
                    }

                    else if (choose == ConsoleKey.D2)
                    {
                        Console.Clear();
                        Console.WriteLine("Malejąco, czy rosnąco?");
                        Console.WriteLine("1. Rosnąco");
                        Console.WriteLine("2. Malejąco");
                        choose = Console.ReadKey().Key;

                        if (choose == ConsoleKey.D1)
                        {
                            Console.Clear();
                            calendar.SortByPriorityAscending();
                        }
                        else if (choose == ConsoleKey.D2)
                        {
                            Console.Clear();
                            calendar.SortByPriorityDescending();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj jeszcze raz.");
                        continue;
                    }
                }
                else if (key == ConsoleKey.D7)
                {
                    Console.Clear();
                    // show task
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (var task in list)
                        {
                            Console.WriteLine($"{++i}. {task.Date.ToShortDateString()}: {task.Name} " +
                                $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                        }
                    }
                    Console.WriteLine("Wpisz numer zadania, które chcesz zakończyć: ");

                    int index = Int32.Parse(Console.ReadLine());
                    while (index >= 0)
                    {
                        if (index < 1 || index > list.Count)
                        {
                            Console.WriteLine("Nie ma takiego zadania. Wybierz numer zadania z listy: ");
                            for (int i = 0; i < list.Count; i++)
                            {
                                foreach (var task in list)
                                {
                                    if (task.IsCompleted == false)
                                    {
                                        Console.WriteLine($"{++i}. {task.Date}: {task.Name} " +
                                        $"({task.TaskPriority}) Done?: {task.IsCompleted}");
                                    }
                                }
                            }
                            index = int.Parse(Console.ReadLine());
                        }
                        else
                        {
                            calendar.EndTask(list.ElementAt(index - 1), GetStatus());
                            break;
                        }
                    }

                }
                else if (key == ConsoleKey.E)
                {
                    Console.Clear();
                    Console.WriteLine("Czy na pewno chcesz wyjść z aplikacji? (t/n)");
                    ConsoleKey exitKey = Console.ReadKey().Key;
                    if (exitKey == ConsoleKey.T)
                    {
                        Console.Clear();
                        Console.WriteLine("Do zobaczenia!");
                    }
                    else if (exitKey == ConsoleKey.N)
                    {
                        Console.Clear();
                        Main();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Niepoprawna opcja, spróbuj jeszcze raz.");
                }
            }
            FileWriter.WriteToFile(filename, list);
        }

        public static string GetTask()
        {
            Console.WriteLine("Proszę dodaj nazwę zadania: ");
            string? taskName = Console.ReadLine();

            // zabezpieczenie przed wprowdzeniem pustego ciągu
            while (taskName != null)
            {
                if (taskName == "")
                {
                    Console.WriteLine("Zadanie nie może być puste. Wpisz co masz do zrobienia: ");
                    taskName = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            return taskName;
        }
        public static DateTime GetDate()
        {
            Console.WriteLine("Wpisz datę: ");
            // zabezpiecznie na wypadek nie wpisania niczego, lub wpisanie nie odpowiedniego formatu daty
            DateTime parsedDate = ParseDate(Console.ReadLine());

            return parsedDate;
        }
        public static Priority GetPriority()
        {
            Console.WriteLine("Wybierz priorytet dla zadania: ");
            Console.WriteLine("1. Wysoki");
            Console.WriteLine("2. Średni");
            Console.WriteLine("3. Niski");

            string? priority = Console.ReadLine();
            int priorityToInt = int.Parse(priority);
            while (priority != null)
            {
                if (priority == "")
                {
                    Console.WriteLine("Nic nie wpisałeś. Wpisz liczbę między 1-3: ");
                    priority = Console.ReadLine();
                }
                else if (priorityToInt < 1 || priorityToInt > 3)
                {
                    Console.WriteLine("Taki priorytet nie istnieje. Wpisz liczbę między 1-3: ");
                    priority = Console.ReadLine();
                    priorityToInt = int.Parse(priority);
                }
                else
                {
                    break;
                }
            }
            priorityToInt = Convert.ToInt32(priority);
            return priorityToInt switch
            {
                1 => Priority.Wysoki,
                2 => Priority.Średni,
                3 => Priority.Niski,
                _ => Priority.Średni
            };
        }
        public static DateTime ParseDate(string date)
        {
            DateTime parsedDate = new DateTime();

            while (date != null)
            {
                if (date.Length < 5 || date.Length > 5)
                {
                    Console.WriteLine("Wpisz datę w poprawnym formacie");
                    date = Console.ReadLine();
                }
                else
                {
                    parsedDate = DateTime.Parse(date);
                    break;
                }
            }
            return parsedDate;
        }

        public static bool GetStatus()
        {
            Console.WriteLine("Czy na pewno chcesz zakończyć zadanie? (t/n)");

            bool status = false;
            ConsoleKey choose = Console.ReadKey().Key;

            if (choose == ConsoleKey.T)
            {
                status = true;
            }
            else if (choose == ConsoleKey.N)
            {
                status = false;
            }

            return status;
        }
    }
}